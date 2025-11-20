using UnityEngine;
using UnityEngine.Events;

public abstract class HealthController : MonoBehaviour
{
    [HideInInspector] protected float _currentHealth = 100f;
    [HideInInspector] protected float _maxHealth = 100f;

    public UnityEvent OnDied;
    public UnityEvent OnDamaged;

    public abstract void TakeDamage(float damageAmount);
    
    public abstract void AddHealth(float amount);

    public float GetCurrentHealth() => _currentHealth;

    public float GetMaxHealth() => _maxHealth;

    public virtual float GetPercentHealth() =>
        _maxHealth > 0 ? _currentHealth / _maxHealth : 0f;

}



