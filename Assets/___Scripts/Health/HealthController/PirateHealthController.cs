using UnityEngine;

public class PirateHealthController : HealthController
{
    [SerializeField] private PirateAttributes _pirateAttributes;

    private void Awake()
    {
        if (_pirateAttributes != null)
        {
            _maxHealth = _pirateAttributes.Health;
            _currentHealth = _pirateAttributes.Health;
        }
    }

    public override void TakeDamage(float damageAmount)
    {
        if (GameManager.Instance.CurrentState != GameState.Gameplay) return;

        if (_currentHealth == 0 || damageAmount == 0) return;

        Debug.Log($"[PirateHealthController] Damage taken {damageAmount}");

        _currentHealth -= damageAmount;

        OnDamaged?.Invoke();

        if (_currentHealth < 0)
            _currentHealth = 0;

        if (_currentHealth == 0)
            OnDied.Invoke();
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
