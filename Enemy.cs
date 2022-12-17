using Godot;
using System;

public class Enemy : KinematicBody2D
{
	[Export] private int _health = 10;
	[Export] private float _speed = 200;
	[Export] private int _damage = 4;
	[Export] private float _attackCooldown = 0.5f;

	private AudioStreamPlayer _audioPlayer;
	private Player _target;
	private bool _isChasing = false;
	private Timer _attackCooldownTimer;
	private bool _readyToAttack = true;
	

	public override void _EnterTree()
	{
		_audioPlayer = GetNode<AudioStreamPlayer>("audioPlayer");
		_attackCooldownTimer = GetNode<Timer>("attackCooldown");
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
			//MoveAndSlide((_target.GlobalPosition - GlobalPosition).Normalized() * _speed);
			var collision = MoveAndCollide((_target.GlobalPosition - GlobalPosition).Normalized() * _speed * delta);

			if (collision != null && collision.Collider is Player && _readyToAttack)
			{
				_readyToAttack = false;
				_attackCooldownTimer.Start(_attackCooldown);
				var player = collision.Collider as Player;
				player.ReceiveDamage(_damage);
			}
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

	private void OnAttackCooldownTimeout()
	{
		_readyToAttack = true;	
	}
}
