using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public class Turret : Area2D
{
	[Export] private float _shootingCooldown = 1f;
	[Export] private int _damage = 1;
	[Export] private Color _lineColor;
	[Export] private int _lineWidth;

	private List<Enemy> _enemies = new List<Enemy>();
	private bool _isReadyToShoot = true;
	private Timer _shootingCooldownTimer;
	private Player _container;

	
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
		_container = GetParent() as Player;

		if (_container != null)
		{
			GD.Print("Container is player");
		} else 
		{
			GD.Print("No container");
		}
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
			_enemies.Remove(enemy);
		}	
	}
	
	private void Shoot()
	{
		if (_enemies.Any() == false || _container == null) return;
		_isReadyToShoot = false;
		_shootingCooldownTimer.Start(_shootingCooldown);
		var line = new PlayerLine();
		line.DefaultColor = _lineColor;
		line.Width = _lineWidth;

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
	
	private void AssignContainer(Player container)
	{
		
		GetParent().CallDeferred("remove_child", this);
		container.CallDeferred("add_child", this);
		Position = Vector2.Zero;
		_container = container;
	}

	public void OnPlayerEntered(object body)
	{
		if (_container != null) return;

		var container = body as Player;

		if (container != null)
		{
			AssignContainer(container);
		}
	}
}










