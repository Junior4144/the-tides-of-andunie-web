using System;
using UnityEngine;
using UnityEngine.Events;

public class PirateHealthController : MonoBehaviour, IHealthController
{
    [SerializeField] private float _currentHealth = 100;
    [SerializeField] private float _maxHealth = 100;

    public UnityEvent OnDied;
    public UnityEvent OnDamaged;

    public void TakeDamage(float damageAmount)
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

    public void AddHealth(float amount)
    {
        if (_currentHealth == _maxHealth || amount == 0)
            return;

        _currentHealth += amount;

        if (_currentHealth > _maxHealth)
            _currentHealth = _maxHealth;
    }

    public float GetPercentHealth() => _currentHealth / _maxHealth;

    public float GetCurrentHealth() => _currentHealth;

    public float GetMaxHealth() => _maxHealth;

    public void SetCurrentHealth(float currentHealth) => _currentHealth = currentHealth;
}
