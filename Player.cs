using Godot;
using System;

public class Player : KinematicBody2D
{
	[Export] public int speed = 200;
	private Vector2 velocity;
	private	const float FLOAT_EPSILON = 20f;
	
	public override void _Ready()
	{
		GetTree().CallGroup("Enemies", "SetPlayer", this);
	}

	public override void _PhysicsProcess(float delta)
	{
		var mousePos = GetGlobalMousePosition();
		var lookVector =  mousePos - GlobalPosition;
		GlobalRotation = Mathf.Atan2(lookVector.y, lookVector.x);
		if (Input.IsActionPressed("move") && GlobalPosition.DistanceTo(mousePos) > FLOAT_EPSILON)
		{
			MoveAndSlide(lookVector.Normalized() * speed);
		}
	}
}