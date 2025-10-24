using UnityEngine;

public class HealthUIController : MonoBehaviour
{
    public static HealthUIController Instance;

    [Header("Health UI")]
    [SerializeField] private HealthBarUI _healthBarUI;
    [SerializeField] private HealthBarShake _healthBarShake;

    private void Awake()
    {
        if (Instance != null) return;

        Instance = this;
    }

    private void OnEnable()
    {
        PlayerHealthController.OnHealthChanged += HandleHealthChanged;
    }

    private void HandleHealthChanged(float _currentHealth, float _maxhealth)
    {
        if (!_healthBarShake.gameObject.activeSelf) return;

        _healthBarShake.Shake();
        _healthBarUI.UpdateHealthBar(_currentHealth, _maxhealth);

    }

    public void UpdateHealthBar(float _currentHealth, float _maxhealth)
    {
        if (!_healthBarUI.gameObject.activeSelf) return;

        _healthBarUI.UpdateHealthBar(_currentHealth, _maxhealth);
    }

    public bool Check_HealthBar_UI_IsActive() => _healthBarUI.gameObject.activeInHierarchy;
}
