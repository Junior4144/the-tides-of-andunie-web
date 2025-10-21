using UnityEngine;

public class InventoryDebugger : MonoBehaviour
{
    [SerializeField] private GameObject testObject;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O)) // ADD
        {
            TryAddFrom(testObject);
            PrintInventory();
        }

        if (Input.GetKeyDown(KeyCode.P)) // REMOVE
        {
            TryRemoveFrom(testObject);
            PrintInventory();
        }

        if (Input.GetKeyDown(KeyCode.K)) // PRINT ONLY
        {
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

    // ----------------- CORE LOGIC -----------------

    void TryAddFrom(GameObject go)
    {
        var data = ExtractItemData(go);
        if (data == null)
        {
            Debug.LogWarning("No IInventoryItem or CollectableData found.");
            return;
        }

        bool added = InventoryManager.Instance.AddItem(data, 1);
        Debug.Log(added
            ? $"✅ Added {data.ItemName}"
            : $"❌ Failed to add {data.ItemName}");
    }

    void TryRemoveFrom(GameObject go)
    {
        var data = ExtractItemData(go);
        if (data == null)
        {
            Debug.LogWarning("No IInventoryItem or CollectableData found.");
            return;
        }

        bool removed = InventoryManager.Instance.RemoveItem(data.ItemId, 1);
        Debug.Log(removed
            ? $"🗑 Removed {data.ItemName}"
            : $"⚠️ Failed to remove {data.ItemName}");
    }

    // ----------------- ITEM EXTRACTION -----------------

    IInventoryItem ExtractItemData(GameObject go)
    {

        // 2) Try any component implementing IInventoryItem
        var invItem = go.GetComponent<IInventoryItem>();
        if (invItem != null)
        {
            return invItem;
        }

        return null;
    }

    // ----------------- PRINT INVENTORY -----------------

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
