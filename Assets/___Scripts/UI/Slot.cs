using UnityEngine;

public class Slot : MonoBehaviour
{
    public IInventoryItem currentItem;

    public void EquipItem()
    {
        InventoryManager.Instance.EquipItem(currentItem.ItemId);
    }
   
}
