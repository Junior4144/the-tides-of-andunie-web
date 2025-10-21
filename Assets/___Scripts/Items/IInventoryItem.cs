using UnityEngine;
public interface IInventoryItem
{
    string ItemId { get; }
    string ItemName { get; }
    Sprite InventoryIconPrefab { get; }
    bool IsStackable { get; }
    int MaxStackSize { get; }
}