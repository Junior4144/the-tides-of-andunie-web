using UnityEngine;
using UnityEngine.Events;

public abstract class HealthController : MonoBehaviour, IHealthController
{
    [SerializeField] protected float _currentHealth = 100;
    [SerializeField] protected float _maxHealth = 100;

    public UnityEvent OnDied;
    public UnityEvent OnDamaged;
    private bool _isShielded;

    public abstract void TakeDamage(float damageAmount);
    public abstract void AddHealth(float amount);

    public float GetCurrentHealth() => _currentHealth;

    public float GetMaxHealth() => _maxHealth;

    public virtual float GetPercentHealth() =>
        _maxHealth > 0 ? _currentHealth / _maxHealth : 0f;

    public void SetCurrentHealth(float currentHealth) =>
        _currentHealth = Mathf.Clamp(currentHealth, 0, _maxHealth);

    public void SetPercentHealth(float percent01)
    {
        percent01 = Mathf.Clamp01(percent01);
        _currentHealth = percent01 * _maxHealth;
    }

    public void AddHealthClamped(float amount)
    {
        _currentHealth = Mathf.Clamp(_currentHealth + amount, 0, _maxHealth);
    }
}


