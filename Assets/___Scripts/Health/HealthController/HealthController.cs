using UnityEngine;
using UnityEngine.Events;

public abstract class HealthController : MonoBehaviour
{
    [HideInInspector] protected float _currentHealth = 100f;
    [HideInInspector] protected float _maxHealth = 100f;

    public UnityEvent OnDied;
    public UnityEvent OnDamaged;

    public virtual void TakeDamage(float damageAmount, DamageType damageType = DamageType.Generic)
    {
        if (GameManager.Instance.CurrentState != GameState.Gameplay) return;
        
        if (_currentHealth == 0 || damageAmount == 0) return;

        _currentHealth -= damageAmount;
        OnDamaged.Invoke();

        _currentHealth = Mathf.Max(0, _currentHealth);

        if (_currentHealth == 0)
            OnDied.Invoke();
    }

    public virtual void AddHealth(float amount)
    {
        if (_currentHealth == _maxHealth || amount == 0) return;

        _currentHealth = Mathf.Clamp(_currentHealth + amount, 0, _maxHealth);
    }

    public float GetCurrentHealth() => _currentHealth;

    public float GetMaxHealth() => _maxHealth;

    public virtual float GetPercentHealth() =>
        _maxHealth > 0 ? _currentHealth / _maxHealth : 0f;

}



