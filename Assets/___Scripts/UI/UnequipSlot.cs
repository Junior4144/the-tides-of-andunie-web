using UnityEngine;

public class UnequipSlot : MonoBehaviour
{

    public IInventoryItem currentItem;

    public void HandlesUnequip()
    {
        if (currentItem == null) return;
        InventoryManager.Instance.UnequipItem(currentItem?.ItemId);
    }
   
}
