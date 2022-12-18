using Godot;
using System;

public class MapGenerator : Node
{
    [Export] private float _grassCap = 0.5f;

    [Export] private Vector2 _roadCaps = new Vector2(0.3f, 0.05f);

    [Export] private Vector3 _enviromentCaps = new Vector3(0.39f, 0.35f, 0.02f);

    private TileMap _grass;
    
    private TileMap _roads;

    private TileMap _enviroment;

    private TileMap _background;

    private OpenSimplexNoise _noise;

    private Random _random;

    private Vector2 _mapSize = new Vector2(500, 500);

    private const int AutoTileNum = 0;

    public override void _Ready()
    {
        _grass = GetParent().GetNode<TileMap>("Grass");
        _roads = GetParent().GetNode<TileMap>("Roads");
        _enviroment = GetParent().GetNode<TileMap>("Enviroment");
        _background = GetParent().GetNode<TileMap>("Background");

        _noise = new OpenSimplexNoise();
        _noise.Octaves = 1;
        _noise.Period = 12f;

        _random = new Random();

        MakeGrass();
        MakeRoads();
        MakeEnvironment();
        MakeBackground();
    }

    private void MakeGrass()
    {
        ApplyMethodToMap((int x, int y) => 
        {
            var noise = _noise.GetNoise2d(x, y);
            if (noise < _grassCap)
            {
                _grass.SetCell(x, y, AutoTileNum);
            }
        });

        UpdateBitmaskRegion(_grass);
    }

    private void MakeRoads()
    {
       ApplyMethodToMap((int x, int y) => 
       {
            var noise = _noise.GetNoise2d(x, y);
            if (noise < _roadCaps.x && noise > _roadCaps.y)
            {
                _roads.SetCell(x, y, AutoTileNum);
            }
       });
        
        UpdateBitmaskRegion(_roads);
    }

    private void MakeEnvironment()
    {
        ApplyMethodToMap((int x, int y) => 
        {
            var noise = _noise.GetNoise2d(x, y);
            if (noise < _enviromentCaps.x && noise > _enviromentCaps.y || noise < _enviromentCaps.z)
            {
                if (_random.Next(0, 99) < 1)
                {
                    var idx = _random.Next(0, 3);
                    _enviroment.SetCell(x, y, idx);
                }
            }
        });
    }

    private void MakeBackground()
    {
        ApplyMethodToMap((int x, int y) => 
        {
            if (_grass.GetCell(x, y) == -1 )
            {
                _background.SetCell(x, y, AutoTileNum);
            }
        });

        UpdateBitmaskRegion(_background);
    }

    private void ApplyMethodToMap(Action<int, int> act)
    {
        for (var x = 0; x < _mapSize.x; x++)
        {
            for (var y = 0; y < _mapSize.y; y++)
            {
                act(x, y);
            }
        }
    }

    private void UpdateBitmaskRegion(TileMap target)
    {
        target.UpdateBitmaskRegion(new Vector2(0, 0), _mapSize);
    }
}
