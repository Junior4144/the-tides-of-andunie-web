using System;
using UnityEngine;

public class PlayerHealthController : HealthController
{
    public static event Action<float, float> OnHealthChanged;

    public override void TakeDamage(float damageAmount)
    {
        if(GameManager.Instance.CurrentState == GameState.Gameplay ||
           GameManager.Instance.CurrentState == GameState.Stage1Gameplay)
        {
            if (_currentHealth == 0 || damageAmount == 0) return;

            _currentHealth -= damageAmount;

            OnDamaged.Invoke();
            OnHealthChanged?.Invoke(_currentHealth, _maxHealth);

            if (_currentHealth < 0)
                _currentHealth = 0;

            if (_currentHealth == 0)
                OnDied.Invoke();
        }

    }

    public override void AddHealth(float amount)
    {
        if (amount == 0)
            return;

        _currentHealth = Mathf.Clamp(_currentHealth + amount, 0, _maxHealth);
        OnHealthChanged?.Invoke(_currentHealth, _maxHealth);
    }
    public void SetCurrentHealth(float currentHealth)
    {
        _currentHealth = Mathf.Clamp(currentHealth, 0, _maxHealth);
        OnHealthChanged?.Invoke(_currentHealth, _maxHealth);
    }
}
