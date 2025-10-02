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
    [SerializeField]
    private float _damageOverTime = 10;
    [SerializeField]

    private float _damageInterval = 1;

    void Start() =>
        StartCoroutine(DamageOverTime());


    public float GetPercentHealth() =>
         _currentHealth / _maxHealth;

    public void TakeDamage(float amount)
    {
        if (_currentHealth == 0)
            return;

        _currentHealth -= amount;

        OnHealthChanged.Invoke();

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

    private IEnumerator DamageOverTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(_damageInterval);
            TakeDamage(_damageOverTime);
        }
    }
}