using Godot;
using System;

public class Enemy : KinematicBody2D
{
	[Export] private int _health = 10;
	[Export] private float _speed = 200;
	
	private AudioStreamPlayer _audioPlayer;
	private Player _target;
	private bool _isChasing = false;
	
	public override void _EnterTree()
	{
		_audioPlayer = GetNode<AudioStreamPlayer>("audioPlayer");
	}
	
	public void ReceiveDamage(int dmg)
	{
		_health -= dmg;
		_audioPlayer.Play();
		
		if (_health <= 0)
		{
			QueueFree();
		}
	}
	
	public override void _PhysicsProcess(float delta)
	{
		if (_isChasing)
		{
			MoveAndSlide((_target.GlobalPosition - GlobalPosition).Normalized() * _speed);
		}
	}
	
	private void OnBodyEntered(object body)
	{
		var target = body as Player;
		if (target != null)
		{
			_target = target;
			_isChasing = true;
		}
	}
	
	private void OnBodyExited(object body)
	{
		_isChasing = false;
	}
}
