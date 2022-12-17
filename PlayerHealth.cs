using Godot;
using System;

public class PlayerHealth : Node
{
    [Export] private int _baseHealth;

    private int _currentHealth;
    private const int HealthCapScale = 2;
    private int _maxHealth;
    public override void _Ready()
    {
        _currentHealth = _baseHealth;
        _maxHealth = HealthCapScale * _baseHealth;
    }

    public int Subtract(int amount)
    {
        _currentHealth -= amount;

        return _currentHealth;
    }

    public int Add(int amount)
    {
        _currentHealth += amount;

        if (_currentHealth > _maxHealth)
        {
            _currentHealth = _maxHealth;
        }

        return _currentHealth;
    }
}
