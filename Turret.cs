using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public class Turret : Area2D
{
	[Export] private float _shootingCooldown = 1f;
	[Export] private int _damage = 1;
	private List<Enemy> _enemies = new List<Enemy>();
	private bool _isReadyToShoot = true;
	private Timer _shootingCooldownTimer;
	
	
	public override void _Process(float del)
	{
		if (_isReadyToShoot)
		{
			Shoot();
		}
	}
	
	public override void _Ready()
	{
		_shootingCooldownTimer = GetNode<Timer>("ShootingCooldown");
	}
	
	private void OnBodyEntered(object body)
	{
		var enemy = body as Enemy;
		if (enemy != null)
		{
			GD.Print("Enemy Entered!");
			_enemies.Add(enemy);
			GD.Print($"{_enemies.Count} enemies now");
		}	
	}

	private void OnBodyExited(object body)
	{
		var enemy = body as Enemy;
		if (enemy != null)
		{
			GD.Print("Enemy Exited!");
			_enemies.Remove(enemy);
			GD.Print($"{_enemies.Count} enemies left");
		}	
	}
	
	private void Shoot()
	{
		if (_enemies.Any() == false) return;
		_isReadyToShoot = false;
		_shootingCooldownTimer.Start(_shootingCooldown);
		var line = new PlayerLine();
		GetTree().Root.AddChild(line);
		
		var enemy = SelectEnemy();
		
		enemy.ReceiveDamage(_damage);		
		
		line.SetLinePoints(GetShootLineCoordinates(enemy));
	}
	
	private Vector2[] GetShootLineCoordinates(Enemy enemy)
	{
		return new Vector2[] { GlobalPosition, enemy.GlobalPosition };
	}
	
	private Enemy SelectEnemy()
	{
		return _enemies
			.OrderBy(e => GlobalPosition.DistanceTo(e.GlobalPosition))
			.First();
	}
	
	private void ShootingCooldownTimeout()
	{
		_isReadyToShoot = true;
	}
}










