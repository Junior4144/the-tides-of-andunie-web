using UnityEngine;
using System.Collections;

public class HealthUIController : MonoBehaviour
{
    [SerializeField] private HealthBarUI _healthBarUI;
    [SerializeField] private HealthBarShake _healthBarShake;

    private HealthController _healthController;

    private void Awake()
    {
        UpdateHealth();
    }

    private void UpdateHealth()
    {



    }
}
