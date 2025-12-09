using TMPro;
using UnityEngine;

public class ShopCurrencyUI : MonoBehaviour
{
    [SerializeField] private TMP_Text coinText;

    private void OnEnable()
    {
        UpdateUI(CurrencyManager.Instance.Coins);

        CurrencyManager.Instance.OnCoinsChanged += UpdateUI;
    }

    private void OnDisable()
    {
        CurrencyManager.Instance.OnCoinsChanged -= UpdateUI;
    }


    private void UpdateUI(int newAmount)
    {
        coinText.text = newAmount.ToString();
    }
}
