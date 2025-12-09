using UnityEngine;

public class Coin : Collectable
{
    [SerializeField] private int _amount;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        HandlePickUp();
        
    }

    public void HandlePickUp()
    {
        CurrencyManager.Instance.AddCoins(_amount);
        PlayPickupSound();
    }
}
