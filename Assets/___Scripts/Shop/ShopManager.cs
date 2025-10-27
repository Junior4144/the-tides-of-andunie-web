using System;
using System.Collections;
using UnityEngine;
//TODO rather than string return, return a ENUM ->(better practice, idk if it worth)

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

    public string TryToBuy(ShopListing listing)
    {
        if (listing == null)
        {
            Debug.LogWarning("No listing was provided to TryBuy.");
            return "NoListingProvided";
        }

        // Check if player can afford
        Debug.Log($"Coins: {CurrencyManager.Instance.Coins}");
        Debug.Log($"Listing Price: {listing.price}");
        if (CurrencyManager.Instance.Coins < listing.price)
        {
            Debug.Log("Not enough coins to buy " + listing.Item.ItemName);
            return "NotEnough";
        }

        // Attempt to add to inventory
        bool added = InventoryManager.Instance.AddItem(listing.Item, listing.quantity);

        if (!added)
        {
            Debug.Log("Inventory full or item cannot be added.");
            return "LimitReached";
        }

        // Deduct coins if added successfully
        CurrencyManager.Instance.TrySpendCoins(listing.price);

        Debug.Log($"Bought {listing.Item.ItemName} x{listing.quantity} for {listing.price} coins");
        return "Success";
    }
}
