using UnityEngine;

public class UnequipSlot : MonoBehaviour
{

    public IInventoryItem currentItem;

    public void HandlesUnequip()
    {
        InventoryManager.Instance.UnequipItem(currentItem?.ItemId);
    }
   
}
