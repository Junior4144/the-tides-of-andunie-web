[System.Serializable]
public class ShopListing
{
    public InventoryItem inventoryItem;
    public int quantity;
    public int price;

    public InventoryItem Item => inventoryItem;
}