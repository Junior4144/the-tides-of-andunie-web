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
        // 1) Try Collectable -> get its data
        var collectable = go.GetComponent<Collectable>();
        if (collectable != null)
        {
            return collectable.Data; // assuming you expose `public CollectableData Data;`
        }

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
