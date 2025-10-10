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

        if (GameManager.Instance.CurrentState != GameState.Gameplay) return;
        // to prevent cutscene from damaging player
        Debug.Log($"GameState: {GameState.Gameplay}");

        if (_currentHealth == 0 || damageAmount == 0)
            return;

        _currentHealth -= damageAmount;

        OnDamaged.Invoke();

        OnHealthChanged.Invoke();

        if (gameObject.CompareTag("Player") && healthBarShake)
            healthBarShake.Shake();

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

        OnHealthChanged.Invoke();

        if (_currentHealth > _maxHealth)
            _currentHealth = _maxHealth;

    }
}