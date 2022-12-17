using Godot;
using System;

public class Player : KinematicBody2D
{
	[Export] private int _speed = 200;
	private	const float FLOAT_EPSILON = 20f;

	private PlayerHealth _health;

	public override void _Ready()
	{
		_health = GetNode<PlayerHealth>("PlayerHealth");
	}

	public override void _PhysicsProcess(float delta)
	{
		var mousePos = GetGlobalMousePosition();
		var lookVector =  mousePos - GlobalPosition;
		GlobalRotation = Mathf.Atan2(lookVector.y, lookVector.x);
		if (Input.IsActionPressed("move") && GlobalPosition.DistanceTo(mousePos) > FLOAT_EPSILON)
		{
			MoveAndSlide(lookVector.Normalized() * _speed);
		}
	}

	public void ReceiveDamage(int damage)
	{
		var playersHealth = _health.Subtract(damage);

		if (playersHealth <= 0)
		{
			GD.Print("Player's dead");
			QueueFree();
		}
	}
}
