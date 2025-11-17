using TMPro;
using UnityEngine;

public class CurrencyUI : MonoBehaviour
{
    [SerializeField] private RectTransform coinContainer;
    [SerializeField] private TMP_Text coinText;

    private void OnEnable()
    {
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

        if (coinContainer != null)
        {
            Vector2 size = coinContainer.sizeDelta;
            size.x = newWidth;
            coinContainer.sizeDelta = size;
             
        }

        if (coinText != null) coinText.text = newAmount.ToString();
    }
}
