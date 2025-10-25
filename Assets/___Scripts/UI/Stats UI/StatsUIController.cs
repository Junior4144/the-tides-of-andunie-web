using TMPro;
using UnityEngine;

public class StatsUIController : MonoBehaviour
{
    [Header("Stat Text References")]
    [SerializeField] private TextMeshProUGUI MaxHpText;
    [SerializeField] private TextMeshProUGUI CurrentHpText;
    [SerializeField] private TextMeshProUGUI DamageText;


    private void OnEnable()
    {
        PlayerHealthController.OnHealthChanged += UpdateHP;
        PlayerStatsManager.OnDamageChanged += UpdateDamage;
    }
    private void OnDisable()
    {
        PlayerHealthController.OnHealthChanged -= UpdateHP;
        PlayerStatsManager.OnDamageChanged -= UpdateDamage;
    }

    private void Start()
    {
        UpdateHP(PlayerManager.Instance.GetHealth(), PlayerStatsManager.Instance.MaxHealth);
        UpdateDamage(PlayerManager.Instance.GetDamageAmount());
    }

    public void UpdateHP(float _currentHealth, float _maxHealth)
    {
        MaxHpText.text = _currentHealth.ToString();
        CurrentHpText.text = _currentHealth.ToString();
    }
    public void UpdateDamage(float value)
    {
        DamageText.text = value.ToString();
    }

}
