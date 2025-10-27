using System;
using UnityEngine;
using UnityEngine.Events;

public class PirateHealthController : HealthController
{
    public override void TakeDamage(float damageAmount)
    {
        if (GameManager.Instance.CurrentState != GameState.Gameplay) return;

        if (_currentHealth == 0 || damageAmount == 0) return;

        _currentHealth -= damageAmount;

        OnDamaged.Invoke(); // Within Scene

        if (_currentHealth < 0)
            _currentHealth = 0;

        if (_currentHealth == 0)
            OnDied.Invoke();
    }

    public override void AddHealth(float amount)
    {
        if (_currentHealth == _maxHealth || amount == 0)
            return;

        _currentHealth += amount;

        if (_currentHealth > _maxHealth)
            _currentHealth = _maxHealth;
    }
}
