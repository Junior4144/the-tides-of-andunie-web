[System.Serializable]
public class ShopListing
{
    public InventoryItem inventoryItem; // must assign a prefab with IInventoryItem
    public int quantity;
    public int price;

    public InventoryItem Item => inventoryItem;
}