using Godot;
using System;

public class PlayerLine : Line2D
{
	private float _lifetime = .500f;

	public override void _Ready()
	{
		AddToGroup("PlayerLine");
	}
	
	public void SetLinePoints(Vector2[] val)
	{
		Points = val;
	}
	
	public override void _PhysicsProcess(float delta)
	{
		_lifetime -= delta;
		
		if (_lifetime < 0)
		{
			QueueFree();
		}
	}
}
