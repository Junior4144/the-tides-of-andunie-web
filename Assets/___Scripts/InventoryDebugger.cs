using UnityEngine;

public class InventoryDebugger : MonoBehaviour
{
    [SerializeField] private GameObject testItemObject;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            IInventoryItem item = testItemObject.GetComponent<IInventoryItem>();
            if (item != null && InventoryManager.Instance.AddItem(item, 1))
                Debug.Log($"Added {item.ItemName}");
            else
                Debug.Log($"Failed to add item");
            
            PrintInventory();
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            IInventoryItem item = testItemObject.GetComponent<IInventoryItem>();
            if (item != null && InventoryManager.Instance.RemoveItem(item.ItemId, 1))
                Debug.Log($"Removed {item.ItemName}");
            else
                Debug.Log($"Failed to remove item");
            
            PrintInventory();
        }
    }

    void PrintInventory()
    {
        var items = InventoryManager.Instance.GetAllItems();
        Debug.Log("=== INVENTORY ===");
        foreach (var slot in items)
        {
            Debug.Log($"{slot.Item.ItemName} x{slot.Quantity}");
        }
        Debug.Log($"Total unique items: {items.Count}");
    }
}