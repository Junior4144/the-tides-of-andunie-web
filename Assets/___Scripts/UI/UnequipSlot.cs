using UnityEngine;

public class UnequipSlot : MonoBehaviour
{

    public InventoryItem currentItem;

    public void HandlesUnequip()
    {
        if (currentItem == null) return;
        InventoryManager.Instance.UnequipItem(currentItem?.ItemId);
    }
   
}
