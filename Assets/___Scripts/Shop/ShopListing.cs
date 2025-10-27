using UnityEngine;

[System.Serializable]
public class ShopListing
{
    public MonoBehaviour inventoryItem; // must assign a prefab with IInventoryItem
    public int quantity;
    public int price;

    public IInventoryItem Item => (IInventoryItem)inventoryItem;
}