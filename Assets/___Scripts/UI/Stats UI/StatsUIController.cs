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
    }
    private void OnDisable()
    {
        PlayerHealthController.OnHealthChanged -= UpdateHP;
    }
    private void Start()
    {
        UpdateHP(PlayerManager.Instance.GetHealth(), PlayerManager.Instance.GetMaxHealth());
    }

    public void UpdateHP(float _currentHealth, float _maxHealth)
    {
        MaxHpText.text = _currentHealth.ToString();
        CurrentHpText.text = _currentHealth.ToString();
    }
    //public void UpdateATK(int value)
    //{
    //    atkText.text = $"ATK: {value}";
    //}

}
