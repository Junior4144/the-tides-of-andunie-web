using TMPro;
using UnityEngine;
using System.Collections;

public class CurrencyUI : MonoBehaviour
{
    [SerializeField] private TMP_Text coinText;
    private bool isSubscribed = false;

    private IEnumerator Start()
    {
        // Wait until the CurrencyManager exists
        while (CurrencyManager.Instance == null)
        {
            yield return null;
        }

        // Subscribe once it's ready
        SubscribeToManager();
        UpdateUI(CurrencyManager.Instance.Coins);
    }

    private void SubscribeToManager()
    {
        if (CurrencyManager.Instance == null || isSubscribed) return;

        CurrencyManager.Instance.OnCoinsChanged += UpdateUI;
        isSubscribed = true;
    }

    private void OnDisable()
    {
        if (CurrencyManager.Instance != null && isSubscribed)
            CurrencyManager.Instance.OnCoinsChanged -= UpdateUI;
    }

    private void UpdateUI(int newAmount)
    {
        if (coinText != null)
            coinText.text = newAmount.ToString();
    }
}
