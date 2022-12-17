using Godot;
using System;

public class MapGenerator : Node
{
    private TileMap _grass;
    private OpenSimplexNoise _noise;
    private float _grassCap = 0.5f;

    private Vector2 _mapSize = new Vector2(500, 500);
    public override void _Ready()
    {
        _grass = GetParent().GetNode<TileMap>("Grass");
        _noise = new OpenSimplexNoise();
        _noise.Octaves = 1;
        _noise.Period = 12f;

        MakeGrass();
    }

    private void MakeGrass()
    {
        for (var x = 0; x < _mapSize.x; x++)
        {
            for (var y = 0; y < _mapSize.y; y++)
            {
                var noise = _noise.GetNoise2d(x, y);
                if (noise < _grassCap)
                {
                    _grass.SetCell(x, y, 0);
                }
            }
        }
    }
}
