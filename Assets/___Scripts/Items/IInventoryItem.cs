using UnityEngine;
public interface IInventoryItem
{
    string ItemId { get; }
    string ItemName { get; }
    //Sprite ItemIcon { get; }
    bool IsStackable { get; }
    int MaxStackSize { get; }
}