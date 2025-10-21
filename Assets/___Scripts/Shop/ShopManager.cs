using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public static ShopManager Instance { get; private set; }

    [SerializeField] private ShopListing[] listings;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public ShopListing[] GetListings() => listings;

    public bool TryToBuy(ShopListing listing)
    {
        if (listing == null)
        {
            Debug.LogWarning("No listing was provided to TryBuy.");
            return false;
        }

        // Check if player can afford
        Debug.Log($"Coins: {CurrencyManager.Instance.Coins}");
        Debug.Log($"Listing Price: {listing.price}");
        if (CurrencyManager.Instance.Coins < listing.price)
        {
            Debug.Log("Not enough coins to buy " + listing.inventoryItem.ItemName);
            return false;
        }

        // Attempt to add to inventory
        bool added = InventoryManager.Instance.AddItem(listing.inventoryItem, listing.quantity);

        if (!added)
        {
            Debug.Log("Inventory full or item cannot be added.");
            return false;
        }

        // Deduct coins if added successfully
        CurrencyManager.Instance.RemoveCoins(listing.price);

        Debug.Log($"Bought {listing.inventoryItem.ItemName} x{listing.quantity} for {listing.price} coins");
        return true;
    }
}
