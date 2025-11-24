using TMPro;
using UnityEngine;

public class ShopCurrencyUI : MonoBehaviour
{
    [SerializeField] private TMP_Text coinText;

    private void OnEnable()
    {
        if (CurrencyManager.Instance != null)
            UpdateUI(CurrencyManager.Instance.Coins);
    }

    private void Start()
    {
        CurrencyManager.Instance.OnCoinsChanged += UpdateUI;
    }

    private void OnDisable()
    {
        if (CurrencyManager.Instance != null)
            CurrencyManager.Instance.OnCoinsChanged -= UpdateUI;
    }

    private void UpdateUI(int newAmount)
    {
        coinText.text = newAmount.ToString();
    }
}
