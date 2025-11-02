using UnityEngine;

public class Slot : MonoBehaviour
{
    public IInventoryItem currentItem;

    public void EquipItem()
    {
        if (currentItem == null) return;
        InventoryManager.Instance.EquipItem(currentItem.ItemId);
    }
   
}
