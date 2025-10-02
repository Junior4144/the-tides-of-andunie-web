using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class HealthController : MonoBehaviour
{
    public UnityEvent OnDied;
    public UnityEvent OnHealthChanged;

    [SerializeField]
    private float _currentHealth = 100;
    [SerializeField]
    private float _maxHealth = 100;

    public float GetPercentHealth() =>
         _currentHealth / _maxHealth;

    public void TakeDamage(float damageAmount)
    {
        if (_currentHealth == 0)
            return;

        _currentHealth -= damageAmount;

        OnHealthChanged.Invoke();

        if (_currentHealth < 0)
            _currentHealth = 0;

        if (_currentHealth == 0)
            OnDied.Invoke();

        Debug.Log($"{_currentHealth}");
    }

    public void AddHealth(float amount)
    {
        if (_currentHealth == _maxHealth)
            return;

        _currentHealth += amount;

        OnHealthChanged.Invoke();

        if (_currentHealth > _maxHealth)
            _currentHealth = _maxHealth;

        Debug.Log($"{_currentHealth}");
    }
}