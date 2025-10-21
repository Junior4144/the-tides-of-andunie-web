using UnityEngine;
using System.Collections.Generic;
using System;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    private readonly Dictionary<string, InventorySlot> _inventory = new();

    public static event Action OnInventoryChanged;

    public ShopListing[] itemPrefabs;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        foreach (var Listing in itemPrefabs) { AddItem(Listing.Item, Listing.quantity); }
    }

    public bool AddItem(IInventoryItem item, int quantity = 1)
    {
        if (item == null || quantity <= 0)
            return false;

        if (_inventory.ContainsKey(item.ItemId))
        {
            InventorySlot slot = _inventory[item.ItemId];
            return slot.CanAddQuantity(quantity) && slot.AddQuantity(quantity);
        }

        if (quantity > item.MaxStackSize)
            return false;

        _inventory[item.ItemId] = new InventorySlot(item, quantity);
        OnInventoryChanged?.Invoke();

        return true;
    }

    public bool RemoveItem(string itemId, int quantity = 1)
    {
        if (!_inventory.ContainsKey(itemId))
            return false;

        InventorySlot slot = _inventory[itemId];
        
        if (!slot.RemoveQuantity(quantity))
            return false;

        if (slot.IsEmpty())
            _inventory.Remove(itemId);

        OnInventoryChanged?.Invoke();
        return true;
    }

    public bool HasItem(string itemId)
    {
        return _inventory.ContainsKey(itemId);
    }

    public int GetItemQuantity(string itemId)
    {
        if (_inventory.ContainsKey(itemId))
            return _inventory[itemId].Quantity;

        return 0;
    }

    public List<InventorySlot> GetAllItems()
    {
        return new List<InventorySlot>(_inventory.Values);
    }

    public void ClearInventory()
    {
        _inventory.Clear();
    }

    public void AddPerk(string perkId) { }
}