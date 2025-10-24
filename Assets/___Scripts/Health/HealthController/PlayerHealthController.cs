using System;
using UnityEngine;
using UnityEngine.Events;

public class PlayerHealthController : HealthController
{
    public static event Action<float, float> OnHealthChanged;
    //public static event Action<string, float, float> OnHealthGained;

    public override void TakeDamage(float damageAmount)
    {
        if (GameManager.Instance.CurrentState != GameState.Gameplay) return;

        if (_currentHealth == 0 || damageAmount == 0) return;

        _currentHealth -= damageAmount;

        OnDamaged.Invoke(); 
        OnHealthChanged?.Invoke(_currentHealth, _maxHealth);

        if (_currentHealth < 0)
            _currentHealth = 0;

        if (_currentHealth == 0)
            OnDied.Invoke();
    }

    public override void AddHealth(float amount)
    {
        if (amount == 0)
            return;

        _currentHealth += amount;

        OnHealthChanged?.Invoke(_currentHealth, _maxHealth);

        if (_currentHealth > _maxHealth)
            _currentHealth = _maxHealth;
    }
}
