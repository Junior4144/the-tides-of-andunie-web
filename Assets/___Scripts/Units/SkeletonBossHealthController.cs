using UnityEngine;

public class SkeletonBossHealthController : HealthController
{
    [SerializeField] private SkeletonBossAttributes _skeletonBossAttributes;

    private void Awake()
    {
        if (_skeletonBossAttributes != null)
        {
            Debug.Log($"[SkeletonBoss] Reading Health from attributes: {_skeletonBossAttributes.Health}");
            _maxHealth = _skeletonBossAttributes.Health;
            _currentHealth = _skeletonBossAttributes.Health;
            Debug.Log($"[SkeletonBoss] Initialized with Health: {_currentHealth}/{_maxHealth}");
        }
        else
        {
            Debug.LogError("[SkeletonBoss] SkeletonBossAttributes is NULL - boss health not set!");
        }
    }

    public override void TakeDamage(float damageAmount)
    {
        if (GameManager.Instance.CurrentState != GameState.Gameplay) return;

        if (_currentHealth == 0 || damageAmount == 0) return;

        float previousHealth = _currentHealth;
        _currentHealth -= damageAmount;
        Debug.Log($"[SkeletonBoss] Took {damageAmount} damage. Health: {previousHealth} → {_currentHealth}/{_maxHealth}");

        OnDamaged?.Invoke();

        if (_currentHealth < 0)
            _currentHealth = 0;

        if (_currentHealth == 0)
        {
            Debug.Log($"[SkeletonBoss] Health reached 0 - triggering death");
            OnDied.Invoke();
        }
    }

    public override void AddHealth(float amount)
    {
        if (_currentHealth == _maxHealth || amount == 0)
            return;

        _currentHealth += amount;

        if (_currentHealth > _maxHealth)
            _currentHealth = _maxHealth;
    }
}
