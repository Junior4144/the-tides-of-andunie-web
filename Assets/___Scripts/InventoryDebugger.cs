using UnityEngine;

public class InventoryDebugger : MonoBehaviour
{
    [SerializeField] private GameObject testItemObject;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O)) // ADD
        {
            IInventoryItem item = testItemObject.GetComponent<IInventoryItem>();
            if (item != null && InventoryManager.Instance.AddItem(item, 1))
                Debug.Log($"Added {item.ItemName}");
            else
                Debug.Log($"Failed to add item");

            PrintInventory();
        }

        if (Input.GetKeyDown(KeyCode.P)) // REMOVE
        {
            IInventoryItem item = testItemObject.GetComponent<IInventoryItem>();
            if (item != null && InventoryManager.Instance.RemoveItem(item.ItemId, 1))
                Debug.Log($"Removed {item.ItemName}");
            else
                Debug.Log($"Failed to remove item");

            PrintInventory();
        }

        if (Input.GetKeyDown(KeyCode.K)) // PRINT ONLY
        {
            PrintInventory();
        }

        if (Input.GetKeyDown(KeyCode.R)) // PRINT ONLY
        {
            Debug.Log($"Amount of Coins: {CurrencyManager.Instance.Coins}");
        }
        if (Input.GetKeyDown(KeyCode.B)) // PRINT ONLY
        {
            CurrencyManager.Instance.AddCoins(1);
            Debug.Log($"Adds 1 Coin: ");
        }
        if (Input.GetKeyDown(KeyCode.N)) // PRINT ONLY
        {
            CurrencyManager.Instance.RemoveCoins(1);
            Debug.Log($"Remove 1 Coin:");
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
