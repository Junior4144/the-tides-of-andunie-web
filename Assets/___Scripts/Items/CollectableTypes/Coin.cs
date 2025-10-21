using UnityEngine;

public class Coin : MonoBehaviour, ICollectable
{
    [SerializeField]
    private int amount;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        Debug.Log("Coin Collider Triggered");
        HandlePickUp();
        Destroy(gameObject);
    }

    public void HandlePickUp() =>
        CurrencyManager.Instance.AddCoins(amount);

}
