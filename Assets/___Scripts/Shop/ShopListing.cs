using UnityEngine;

[CreateAssetMenu(menuName = "Shop/Listing")]
public class ShopListing : ScriptableObject
{
    public ShopItem inventoryItem;
    public int price;
    public int quantity = 1;
}
