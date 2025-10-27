using UnityEngine;
using System.Linq;

public class InventoryDebugger : MonoBehaviour
{
    [Header("Debug Settings")]
    [SerializeField] private GameObject itemPrefabToEquip;
    [SerializeField] private int quantityToEquip = 1;
    [SerializeField] private int quantityToAdd = 1;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
            DebugAddItem();

        if (Input.GetKeyDown(KeyCode.O))
            DebugEquipItem();

        if (Input.GetKeyDown(KeyCode.P))
            DebugUnequipItem();

        if (Input.GetKeyDown(KeyCode.I))
            DebugRemoveItem();

        if (Input.GetKeyDown(KeyCode.K))
            PrintInventory();

        if (Input.GetKeyDown(KeyCode.L))
            PrintPlayerStats();
    }

    private void DebugAddItem()
    {
        if (itemPrefabToEquip == null)
        {
            Debug.LogWarning("No item prefab assigned to debugger!");
            return;
        }

        IInventoryItem item = itemPrefabToEquip.GetComponent<IInventoryItem>();
        
        if (item == null)
        {
            Debug.LogError("Prefab doesn't have IInventoryItem component!");
            return;
        }

        bool success = InventoryManager.Instance.AddItem(item, quantityToAdd);
        
        if (success)
        {
            Debug.Log($"<color=cyan>Added {item.ItemName} x{quantityToAdd} to inventory</color>");
            PrintInventory();
        }
        else
        {
            Debug.LogWarning($"<color=red>Failed to add {item.ItemName}</color>");
        }
    }

    private void DebugRemoveItem()
    {
        if (itemPrefabToEquip == null)
        {
            Debug.LogWarning("No item prefab assigned to debugger!");
            return;
        }

        IInventoryItem item = itemPrefabToEquip.GetComponent<IInventoryItem>();
        
        if (item == null)
            return;

        bool success = InventoryManager.Instance.RemoveItem(item.ItemId, quantityToAdd);
        
        if (success)
        {
            Debug.Log($"<color=orange>Removed {item.ItemName} x{quantityToAdd} from inventory</color>");
            PrintInventory();
        }
        else
        {
            Debug.LogWarning($"<color=red>Failed to remove {item.ItemName}</color>");
        }
    }

    private void DebugEquipItem()
    {
        if (itemPrefabToEquip == null)
        {
            Debug.LogWarning("No item prefab assigned to debugger!");
            return;
        }

        IInventoryItem item = itemPrefabToEquip.GetComponent<IInventoryItem>();
        
        if (item == null)
        {
            Debug.LogError("Prefab doesn't have IInventoryItem component!");
            return;
        }

        // Check if item is in inventory
        if (!InventoryManager.Instance.HasItem(item.ItemId))
        {
            Debug.LogWarning($"{item.ItemName} not in inventory! Add it first with 'A' key.");
            return;
        }

        // Try to equip
        bool success = InventoryManager.Instance.EquipItem(item.ItemId, quantityToEquip);
        
        if (success)
        {
            Debug.Log($"<color=green>Successfully equipped {item.ItemName} x{quantityToEquip}</color>");
            PrintInventory();
        }
        else
        {
            Debug.LogWarning($"<color=red>Failed to equip {item.ItemName}</color>");
        }
    }

    private void DebugUnequipItem()
    {
        if (itemPrefabToEquip == null)
        {
            Debug.LogWarning("No item prefab assigned to debugger!");
            return;
        }

        IInventoryItem item = itemPrefabToEquip.GetComponent<IInventoryItem>();
        
        if (item == null)
            return;

        bool success = InventoryManager.Instance.UnequipItem(item.ItemId, quantityToEquip);
        
        if (success)
        {
            Debug.Log($"<color=yellow>Unequipped {item.ItemName} x{quantityToEquip}</color>");
            PrintInventory();
        }
        else
        {
            Debug.LogWarning($"<color=red>Failed to unequip {item.ItemName}</color>");
        }
    }

    private void PrintInventory()
    {
        Debug.Log("========== INVENTORY DEBUG ==========");
        
        // Print Inventory
        Debug.Log("<b>INVENTORY:</b>");
        var allItems = InventoryManager.Instance.GetAllItems();
        
        if (allItems.Count == 0)
        {
            Debug.Log("  (empty)");
        }
        else
        {
            allItems.ForEach(slot => 
                Debug.Log($"  • {slot.Item.ItemName} x{slot.Quantity}"));
        }

        // Print Equipped Items
        Debug.Log("<b>EQUIPPED ITEMS:</b>");
        var equippedItems = InventoryManager.Instance.GetEquippedItems();
        
        if (equippedItems.Count == 0)
        {
            Debug.Log("  (none)");
        }
        else
        {
            equippedItems.ForEach(slot => 
                Debug.Log($"  • {slot.Item.ItemName} x{slot.Quantity}"));
        }

        Debug.Log($"<b>EQUIPPED SLOTS:</b> {InventoryManager.Instance.EquippedItemCount}/{InventoryManager.Instance.MaxEquippedSlots}");
        Debug.Log("=====================================");
    }

    private void PrintPlayerStats()
    {
        Debug.Log("========== PLAYER STATS ==========");
        Debug.Log($"Melee Damage: {PlayerStatsManager.Instance.MeleeDamage}");
        Debug.Log($"Max Health: {PlayerStatsManager.Instance.MaxHealth}");
        Debug.Log("==================================");
    }
}