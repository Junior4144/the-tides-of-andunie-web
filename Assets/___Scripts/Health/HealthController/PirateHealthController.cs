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

}
