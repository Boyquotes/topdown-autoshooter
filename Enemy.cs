using Godot;
using System;

public class Enemy : KinematicBody2D
{
	[Export] private int _health = 10;
	[Export] private float _speed = 200;
	[Export] private float _detectionRange = 500;
	
	private AudioStreamPlayer _audioPlayer;
	private Player _player;
	
	public override void _EnterTree()
	{
		_audioPlayer = GetNode<AudioStreamPlayer>("audioPlayer");
		AddToGroup("Enemies");
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
	
	public void SetPlayer(Player player)
	{
		_player = player;
		
		GD.Print("player set");
	}
	
	public override void _PhysicsProcess(float delta)
	{
		if (_player == null) return;
		
		if (GlobalPosition.DistanceTo(_player.GlobalPosition) < _detectionRange)
		{
			MoveAndSlide((_player.GlobalPosition - GlobalPosition).Normalized() * _speed);
		}
	}
}
