using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    private readonly Dictionary<string, InventorySlot> _inventory = new();
    private readonly Dictionary<string, InventorySlot> _equippedItems = new();

    [SerializeField] private int _maxEquippedSlots = 3;

    public static event Action OnInventoryChanged;
    public static event Action OnEquippedItemsChanged;

    public int MaxEquippedSlots => _maxEquippedSlots;
    public int EquippedItemCount => _equippedItems.Count;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public bool AddItem(IInventoryItem item, int quantity = 1)
    {
        if (!IsValidAddition(item, quantity)) return false;
        if (HasItem(item.ItemId))
        {
            bool result =  AddToExistingSlot(item.ItemId, quantity);
            OnInventoryChanged?.Invoke();
            return result;
        }

        bool res = CreateNewSlot(item, quantity);
        OnInventoryChanged?.Invoke();
        return res;
    }

    public bool RemoveItem(string itemId, int quantity = 1)
    {
        if (!HasItem(itemId)) return false;

        var inventorySlot = _inventory[itemId];
        if (!inventorySlot.RemoveQuantity(quantity)) return false;

        SyncEquippedQuantityWithInventory(itemId, inventorySlot);
        RemoveEmptySlot(itemId, inventorySlot);
        
        OnInventoryChanged?.Invoke();
        return true;
    }

    public bool EquipItem(string itemId, int quantity = 1)
    {
        if (!CanEquip(itemId, quantity)) return false;

        var inventorySlot = _inventory[itemId];
        if (!AddToEquippedSlot(itemId, inventorySlot, quantity)) return false;

        RecalculateStats();
        OnEquippedItemsChanged?.Invoke();
        return true;
    }

    public bool UnequipItem(string itemId, int quantity = 1)
    {
        if (!HasItemEquipped(itemId)) return false;

        var slot = _equippedItems[itemId];
        if (quantity > slot.Quantity || !slot.RemoveQuantity(quantity)) return false;

        if (slot.IsEmpty()) _equippedItems.Remove(itemId);

        RecalculateStats();
        OnEquippedItemsChanged?.Invoke();
        return true;
    }

    private bool IsValidAddition(IInventoryItem item, int quantity) =>
        item != null && quantity > 0;

    private bool AddToExistingSlot(string itemId, int quantity)
    {
        var slot = _inventory[itemId];
        return slot.CanAddQuantity(quantity) && slot.AddQuantity(quantity);
    }

    private bool CreateNewSlot(IInventoryItem item, int quantity)
    {
        if (quantity > item.MaxStackSize) return false;
        
        _inventory[item.ItemId] = new InventorySlot(item, quantity);
        OnInventoryChanged?.Invoke();
        return true;
    }

    private void SyncEquippedQuantityWithInventory(string itemId, InventorySlot inventorySlot)
    {
        if (!HasItemEquipped(itemId)) return;

        var equippedSlot = _equippedItems[itemId];
        if (equippedSlot.Quantity > inventorySlot.Quantity)
            UnequipItem(itemId, equippedSlot.Quantity - inventorySlot.Quantity);
    }

    private void RemoveEmptySlot(string itemId, InventorySlot slot)
    {
        if (!slot.IsEmpty()) return;

        if (HasItemEquipped(itemId)) UnequipAllOfItem(itemId);
        _inventory.Remove(itemId);
    }

    private bool CanEquip(string itemId, int quantity) =>
        HasItem(itemId) && 
        (HasItemEquipped(itemId) || EquippedItemCount < _maxEquippedSlots) &&
        GetEquippedQuantity(itemId) + quantity <= _inventory[itemId].Quantity;

    private bool AddToEquippedSlot(string itemId, InventorySlot inventorySlot, int quantity)
    {
        if (HasItemEquipped(itemId))
        {
            var equippedSlot = _equippedItems[itemId];
            return equippedSlot.CanAddQuantity(quantity) && equippedSlot.AddQuantity(quantity);
        }

        _equippedItems[itemId] = new InventorySlot(inventorySlot.Item, quantity);
        return true;
    }

    private void RecalculateStats()
    {
        PlayerStatsManager.Instance.ResetToDefaults();
        _equippedItems.Values.ToList().ForEach(ApplySlotEffects);
    }

    private void ApplySlotEffects(InventorySlot slot)
    {
        var effects = slot.Item.GetEffects();
        if (effects == null || effects.Length == 0) return;

        var sortedEffects = effects
            .OrderBy(e => e.IsPercentage)
            .ToList();

        sortedEffects.ForEach(effect =>
            Enumerable.Range(0, slot.Quantity).ToList().
                ForEach(_ => effect.Apply()));
    }

    private void UnequipAllOfItem(string itemId)
    {
        if (HasItemEquipped(itemId))
            UnequipItem(itemId, _equippedItems[itemId].Quantity);
    }

    public int GetEquippedQuantity(string itemId) => 
        HasItemEquipped(itemId) ? _equippedItems[itemId].Quantity : 0;

    public bool HasItemEquipped(string itemId) => _equippedItems.ContainsKey(itemId);

    public List<InventorySlot> GetEquippedItems() => new(_equippedItems.Values);

    public bool CanEquipMoreItems() => EquippedItemCount < _maxEquippedSlots;

    public bool HasItem(string itemId) => _inventory.ContainsKey(itemId);

    public int GetItemQuantity(string itemId) => 
        _inventory.ContainsKey(itemId) ? _inventory[itemId].Quantity : 0;

    public List<InventorySlot> GetAllItems() => new(_inventory.Values);

    public void ClearInventory()
    {
        _equippedItems.Keys.ToList().ForEach(UnequipAllOfItem);
        _inventory.Clear();
    }
}