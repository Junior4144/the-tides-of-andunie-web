using TMPro;
using UnityEngine;

public class CurrencyUI : MonoBehaviour
{
    [SerializeField] private RectTransform coinContainer;
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
        
        int baseWidthPerDigit = 40;
        
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
