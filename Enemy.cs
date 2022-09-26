using Godot;
using System;

public class Enemy : KinematicBody2D
{
	[Export] private int _health = 10;
	
	private AudioStreamPlayer _audioPlayer;
	
	public override void _Ready()
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
}
