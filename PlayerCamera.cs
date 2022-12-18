using Godot;
using System;

public class PlayerCamera : Camera2D
{
    [Export] private float _maxZoom = 5.0f;
    [Export] private float _minZoom = 1f;
    [Export] private float _zoomFactor = 1f;
    [Export] private float _zoomDuration = 0.2f;
//var _zoom_level := 1.0 setget _set_zoom_level

    
    private Node2D _target;
    private Tween _tween;
    private float _zoomLevel = 1.0f;


    public override void _Ready()
    {
        _target = GetParent() as Node2D;
        _tween = GetNode<Tween>("Tween");
    }

    public override void _PhysicsProcess(float delta)
    {
     //   Position = _target.Position;
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event.IsActionPressed("zoom_in"))
        {
            SetZoomLevel(_zoomLevel - _zoomFactor);
        }

        if (@event.IsActionPressed("zoom_out"))
        {
            SetZoomLevel(_zoomLevel + _zoomFactor);
        }
    }
    private void SetZoomLevel(float value)
    {
        _zoomLevel = ClampZoom(value);

        _tween.InterpolateProperty(this, "zoom", Zoom, new Vector2(_zoomLevel, _zoomLevel), _zoomDuration, Tween.TransitionType.Sine, Tween.EaseType.Out);

        _tween.Start();
    }

    private float ClampZoom(float value)
    {
        if (value > _maxZoom)
        {
            return _maxZoom;
        }

        if (value < _minZoom)
        {
            return _minZoom;
        }

        return value;
    }
}
