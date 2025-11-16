using UnityEngine;
public interface IInventoryItem
{
    string ItemId { get; }
    string ItemName { get; }
    GameObject InventoryIconPrefab { get; }
    bool IsStackable { get; }
    int MaxStackSize { get; }
    Sprite SpriteIcon { get; }
    ItemEffect[] GetEffects();
}