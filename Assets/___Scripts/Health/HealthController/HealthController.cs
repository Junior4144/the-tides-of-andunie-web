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

    public float GetPercentHealth() => _currentHealth / _maxHealth;

    public float GetCurrentHealth() => _currentHealth;

    public void SetCurrentHealth(float currentHealth) => _currentHealth = currentHealth;
}
