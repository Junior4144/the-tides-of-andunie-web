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
        UpdateHP(PlayerManager.Instance.GetHealth(), 0f);
        UpdateMaxHP(PlayerStatsManager.Instance.MaxHealth);
        UpdateDamage(PlayerStatsManager.Instance.MeleeDamage);

        PlayerHealthController.OnHealthChanged += UpdateHP;
        PlayerStatsManager.OnDamageChanged += UpdateDamage;
        PlayerStatsManager.OnMaxHealthChanged += UpdateMaxHP;
    }
    private void OnDisable()
    {
        PlayerHealthController.OnHealthChanged -= UpdateHP;
        PlayerStatsManager.OnDamageChanged -= UpdateDamage;
        PlayerStatsManager.OnMaxHealthChanged -= UpdateMaxHP;
    }

    public void UpdateHP(float currentHealth, float _)
    {
        CurrentHpText.text = currentHealth.ToString();
    }

    public void UpdateMaxHP(float maxHealth)
    {
        MaxHpText.text = maxHealth.ToString();
    }

    public void UpdateDamage(float value)
    {
        DamageText.text = value.ToString();
    }

}
