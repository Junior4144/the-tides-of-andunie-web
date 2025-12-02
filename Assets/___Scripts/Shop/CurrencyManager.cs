using UnityEngine;
using System;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance { get; private set; }

    [SerializeField]
    public int Coins { get; private set; }
    public event Action<int> OnCoinsChanged;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void AddCoins(int amount)
    {
        Coins += amount;
        OnCoinsChanged?.Invoke(Coins);
        SaveGameManager.Instance.SaveCoinData();
        Debug.Log($"[Curreny Manager] Coins: {Coins}");
    }

    public bool TrySpendCoins(int amount)
    {
        if (Coins < amount) return false;
        Coins -= amount;
        OnCoinsChanged?.Invoke(Coins);
        SaveGameManager.Instance.SaveCoinData();
        return true;
    }

    public void SetCoins(int amount)
    {
        Coins = amount;
        OnCoinsChanged?.Invoke(Coins);
        Debug.Log($"[CurrencyManager] Loaded coins = {Coins}");
    }
}


