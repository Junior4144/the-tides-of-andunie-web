using UnityEngine;

public class ShopItem : IInventoryItem
{
    [SerializeField] string itemId;
    [SerializeField] string itemName;
    [SerializeField] bool stackable = true;
    [SerializeField] int maxStackSize = 20;
    [SerializeField] private GameObject inventoryIconPrefab;

    public string ItemId => itemId;
    public string ItemName => itemName;
    public bool IsStackable => stackable;
    public int MaxStackSize => maxStackSize;
    public GameObject InventoryIconPrefab => inventoryIconPrefab;

}
