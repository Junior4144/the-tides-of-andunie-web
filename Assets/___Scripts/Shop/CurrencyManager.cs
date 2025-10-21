using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance { get; private set; }

    public int Coins { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public void AddCoins(int amount) => Coins += amount;
    public void RemoveCoins(int amount) => Coins -= amount;
}