using Godot;
using System;

public class MapGenerator : Node
{
    [Export] private float _grassCap = 0.5f;
    [Export] private Vector2 _roadCaps = new Vector2(0.3f, 0.05f);
    [Export] private Vector3 _enviromentCaps = new Vector3(0.4f, 0.3f, 0.04f);

    private TileMap _grass;
    private TileMap _roads;

    private OpenSimplexNoise _noise;


    private Vector2 _mapSize = new Vector2(500, 500);
    public override void _Ready()
    {
        _grass = GetParent().GetNode<TileMap>("Grass");
        _roads = GetParent().GetNode<TileMap>("Roads");

        _noise = new OpenSimplexNoise();
        _noise.Octaves = 1;
        _noise.Period = 12f;

        MakeGrass();
        MakeRoads();
    }

    private void MakeGrass()
    {
        ApplyMethodToMap((float noise, int x, int y) => {
            if (noise < _grassCap)
            {
                _grass.SetCell(x, y, 0);
            }
        });

        UpdateBitmaskRegion(_grass);
    }

    private void MakeRoads()
    {
       ApplyMethodToMap((float noise, int x, int y) => {
            if (noise < _roadCaps.x && noise > _roadCaps.y)
            {
                _roads.SetCell(x, y, 0);
            }
       });
        
        UpdateBitmaskRegion(_roads);
    }

    private void ApplyMethodToMap(Action<float, int, int> act)
    {
        for (var x = 0; x < _mapSize.x; x++)
        {
            for (var y = 0; y < _mapSize.y; y++)
            {
                var noise = _noise.GetNoise2d(x, y);
                act(noise, x, y);
            }
        }
    }

    private void UpdateBitmaskRegion(TileMap target)
    {
        target.UpdateBitmaskRegion(new Vector2(0, 0), _mapSize);
    }
}
