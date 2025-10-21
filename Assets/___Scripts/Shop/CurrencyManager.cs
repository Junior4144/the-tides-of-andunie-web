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
            Destroy(gameObject); //removes item from game once collected
            return;
        }

        Instance = this;

        // Must be a root object to persist correctly
        if (transform.parent != null)

            DontDestroyOnLoad(gameObject);
    }

    public void AddCoins(int amount)
    {
        Coins += amount;
        OnCoinsChanged?.Invoke(Coins); //increment coin counter 
    }

    public bool TrySpendCoins(int amount)
    {
        if (Coins < amount) return false;
        Coins -= amount;
        OnCoinsChanged?.Invoke(Coins); //same function as remove coins just new name
        return true;
    }
}


