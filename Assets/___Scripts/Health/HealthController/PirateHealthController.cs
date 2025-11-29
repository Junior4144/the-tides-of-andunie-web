using UnityEngine;

public class PirateHealthController : HealthController
{
    [SerializeField] private PirateAttributes _pirateAttributes;

    private ImpulseController _impulseController;
    private bool _isDead = false;
    private bool shouldDie = false;

    private void Awake()
    {
        if (_pirateAttributes != null)
        {
            _maxHealth = _pirateAttributes.Health;
            _currentHealth = _pirateAttributes.Health;
        }

        _impulseController = GetComponent<ImpulseController>();
    }

    private void FixedUpdate()
    {

        if (shouldDie && !_impulseController.IsInImpulse() && !_isDead)
        {
            _isDead = true;
            OnDied.Invoke();
        }
    }

}
