using UnityEngine;
using System;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance { get; private set; }

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
    }

    public bool TrySpendCoins(int amount)
    {
        if (Coins < amount) return false;
        Coins -= amount;
        OnCoinsChanged?.Invoke(Coins);
        return true;
    }
}


