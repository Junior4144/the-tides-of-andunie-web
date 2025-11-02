using TMPro;
using UnityEngine;

public class StatsUIController : MonoBehaviour
{
    [Header("Stat Text References")]
    [SerializeField] private TextMeshProUGUI MaxHpText;
    [SerializeField] private TextMeshProUGUI MaxHpAdditiveText;


    [SerializeField] private TextMeshProUGUI DamageText;
    [SerializeField] private TextMeshProUGUI DamageAdditiveText;

    private void OnEnable()
    {


        PlayerStatsManager.OnDamageChanged += UpdateDamage;
        PlayerStatsManager.OnMaxHealthChanged += UpdateMaxHP;

        if (PlayerStatsManager.Instance != null)
        {
            UpdateMaxHP(PlayerStatsManager.Instance.MaxHealth, PlayerStatsManager.Instance.DefaultMaxHealth);
            UpdateDamage(PlayerStatsManager.Instance.MeleeDamage, PlayerStatsManager.Instance.DefaultMeleeDamage);
        }
    }
    private void OnDisable()
    {
        PlayerStatsManager.OnDamageChanged -= UpdateDamage;
        PlayerStatsManager.OnMaxHealthChanged -= UpdateMaxHP;
    }


    public void UpdateMaxHP(float maxHealth, float defaultMaxHealth)
    {
        float additive = maxHealth - defaultMaxHealth;

        MaxHpText.text = maxHealth.ToString();

        if (additive > 0)
            MaxHpAdditiveText.text = $"(+{additive})";
        else if (additive < 0)
            MaxHpAdditiveText.text = $"({additive})";
        else
            MaxHpAdditiveText.text = "";
    }

    public void UpdateDamage(float damage, float defaultDamage)
    {
        float additive = damage - defaultDamage;
        Debug.Log($"[STATSUICONTROLLER] damage: {damage}");
        DamageText.text = damage.ToString();

        if (additive > 0)
            DamageAdditiveText.text = $"(+{additive})";
        else if (additive < 0)
            DamageAdditiveText.text = $"({additive})";
        else
            DamageAdditiveText.text = "";
    }

}
