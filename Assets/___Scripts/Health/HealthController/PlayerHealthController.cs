using System;
using UnityEngine;

public class PlayerHealthController : HealthController
{
    public static event Action<float, float> OnHealthChanged;

    public override void TakeDamage(float damageAmount, DamageType damageType = DamageType.Generic)
    {
        if(GameManager.Instance.CurrentState == GameState.Gameplay ||
           GameManager.Instance.CurrentState == GameState.Stage1Gameplay)
        {
            if (_currentHealth == 0 || damageAmount == 0) return;

            float resistance = GetResistanceForDamageType(damageType);
            float damageMultiplier = CalculateDamageMultiplier(resistance);
            float finalDamage = damageAmount * damageMultiplier;

            _currentHealth -= finalDamage;

            OnDamaged.Invoke();
            OnHealthChanged?.Invoke(_currentHealth, _maxHealth);

            if (_currentHealth < 0)
                _currentHealth = 0;

            if (_currentHealth == 0)
                OnDied.Invoke();
        }

    }

    private float GetResistanceForDamageType(DamageType damageType)
    {
        return damageType switch
        {
            DamageType.Melee => PlayerStatsManager.Instance.MeleeResistance,
            DamageType.Ranged => PlayerStatsManager.Instance.RangedResistance,
            DamageType.Explosion => PlayerStatsManager.Instance.ExplosionResistance,
            DamageType.Generic => 0f,
            _ => 0f
        };
    }

    private float CalculateDamageMultiplier(float resistance)
    {
        if (resistance >= 0)
        {
            // Positive resistance: diminishing returns, asymptotically approaches 0
            return 100f / (100f + resistance);
        }
        else
        {
            // Negative resistance: linear increase in damage taken
            return 1f - (resistance / 100f);
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
