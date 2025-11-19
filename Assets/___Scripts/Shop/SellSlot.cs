using UnityEngine;

public class SellSlot : MonoBehaviour
{
    public IInventoryItem currentItem;

    public void SellItem()
    {
        if (currentItem == null) return;

        ShopManager.Instance.TryToSell(currentItem);
    }
}
