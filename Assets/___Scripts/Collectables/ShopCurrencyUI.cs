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
        int digitCount = newAmount.ToString().Length;

        // Each digit = 40 width
        int baseWidthPerDigit = 40;

        // Max 4 digits -> 160 width
        int newWidth = Mathf.Min(digitCount * baseWidthPerDigit, baseWidthPerDigit * 4);

        if (coinText != null) coinText.text = newAmount.ToString();
    }
}
