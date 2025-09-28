using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class HealthController : MonoBehaviour
{
    // Allows you to change private variables in Unity
    [SerializeField]
    private float _currentHealth = 100;

    [SerializeField]
    private float _maxHealth = 100;

    [SerializeField]
    private float _damageOverTime = 10;

    [SerializeField]
    // In seconds
    private float _damageInterval = 1;

    void Start()
    {
        StartCoroutine(DamageOverTime());
    }

    public float GetPercentHealth()
    {
        return _currentHealth / _maxHealth;
    }

    public UnityEvent OnDied;

    public void TakeDamage(float amount)
    {
        if (_currentHealth == 0)
        {
            return;
        }

        _currentHealth -= amount;

        if (_currentHealth < 0)
        {
            _currentHealth = 0;
        }

        if (_currentHealth == 0)
        {
            OnDied.Invoke();
        }
    }

    public void AddHealth(float amount)
    {
        if (_currentHealth == _maxHealth)
        {
            return;
        }

        _currentHealth += amount;

        if (_currentHealth > _maxHealth)
        {
            _currentHealth = _maxHealth;
        }
    }

    // Necessary for execution across multiple frames
    private IEnumerator DamageOverTime()
    {
        // So units can suffer from damage over time effects
        while (true)
        {
            yield return new WaitForSeconds(_damageInterval);
            TakeDamage(_damageOverTime);
        }
    }
}