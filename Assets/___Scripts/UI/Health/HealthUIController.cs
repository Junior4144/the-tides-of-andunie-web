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
    private void OnDisable()
    {
        PlayerHealthController.OnHealthChanged -= HandleHealthChanged;
    }

    private void HandleHealthChanged(float currentHealth, float maxhealth)
    {
        if (!_healthBarShake.gameObject.activeSelf) return;

        _healthBarShake.Shake();
        _healthBarUI.UpdateHealthBar(currentHealth, maxhealth);

    }

    public void UpdateHealthBar(float currentHealth, float maxhealth)
    {
        if (Check_HealthBar_UI_IsActive()) return;

        _healthBarUI.UpdateHealthBar(currentHealth, maxhealth);
    }

    public bool Check_HealthBar_UI_IsActive() => _healthBarUI.gameObject.activeInHierarchy;
}
