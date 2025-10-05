using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class HealthController : MonoBehaviour
{
    public UnityEvent OnDied;
    public UnityEvent OnHealthChanged;
    public UnityEvent OnDamaged;

    [SerializeField] private float _currentHealth = 100;
    [SerializeField] private float _maxHealth = 100;
    [SerializeField] private HealthBarShake healthBarShake;

    public float GetPercentHealth() =>
         _currentHealth / _maxHealth;

    public void TakeDamage(float damageAmount)
    {
        if (_currentHealth == 0)
            return;

        _currentHealth -= damageAmount;

        OnDamaged.Invoke();

        OnHealthChanged.Invoke();

        if (this.CompareTag("Player"))
            healthBarShake.Shake();

        if (_currentHealth < 0)
            _currentHealth = 0;

        if (_currentHealth == 0)
            OnDied.Invoke();
    }

    public void AddHealth(float amount)
    {
        if (_currentHealth == _maxHealth)
            return;

        _currentHealth += amount;

        OnHealthChanged.Invoke();

        if (_currentHealth > _maxHealth)
            _currentHealth = _maxHealth;

    }
}