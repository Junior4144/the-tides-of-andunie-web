using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class EquipUIController : MonoBehaviour
{
    public GameObject InventoryPanel;
    public GameObject slotPrefab;
    public int slotCount;
    public InventoryItem[] inventoryItems;

    void Awake()
    {
        inventoryItems = new InventoryItem[slotCount];
        InitializeSlots();

    }

    void OnEnable()
    {
        InventoryManager.OnEquippedItemsChanged += RefreshUI;
        RefreshUI(); // <<-- so UI updates immediately when opening
    }

    void OnDisable()
    {
        InventoryManager.OnEquippedItemsChanged -= RefreshUI;
    }

    private void InitializeSlots()
    {
        // Prevent double-creation if already filled
        if (InventoryPanel.transform.childCount > 0) return;

        for (int i = 0; i < slotCount; i++)
        {
            var slot = Instantiate(slotPrefab, InventoryPanel.transform).GetComponent<UnequipSlot>();
            if (inventoryItems[i] != null)
            {
                GameObject item = Instantiate(inventoryItems[i].InventoryIconPrefab, slot.transform);
                RectTransform rect = item.GetComponent<RectTransform>();
                rect.anchoredPosition = Vector2.zero;
                rect.localScale = Vector3.one;
                slot.currentItem = inventoryItems[i];
            }
        }

        // ✅ Force Unity to rebuild the layout once all slots exist
        LayoutRebuilder.ForceRebuildLayoutImmediate(InventoryPanel.GetComponent<RectTransform>());
        Debug.Log($"[InventoryController] Initialized {slotCount} slots.");
    }

    private void RefreshUI()
    {
        if (!InventoryPanel)
        {
            Debug.LogError("[InventoryUI] InventoryPanel is null.");
            return;
        }

        Debug.Log("[InventoryUI] RefreshUI start");

        // 1) Clear all current item visuals
        int cleared = 0;
        foreach (Transform slotTransform in InventoryPanel.transform)
        {
            var slot = slotTransform.GetComponent<UnequipSlot>();
            if (!slot)
            {
                Debug.LogWarning($"[InventoryUI] Child '{slotTransform.name}' has no Slot component.");
                continue;
            }

            // Destroy all child GameObjects inside this slot
            for (int i = slotTransform.childCount - 1; i >= 0; i--)
            {
                Destroy(slotTransform.GetChild(i).gameObject);
                cleared++;
            }

            slot.currentItem = null;
        }
        Debug.Log($"[InventoryUI] Cleared {cleared} items");

        // 2) Rebuild from inventory data
        if (!InventoryManager.Instance) return;
        var items = InventoryManager.Instance.GetEquippedItems();
        if (items == null)
        {
            Debug.LogError("[InventoryUI] InventoryManager returned null list.");
            return;
        }
        Debug.Log($"[InventoryUI] Items to draw: {items.Count}");

        int slotIndex = 0;
        for (int i = 0; i < items.Count; i++)
        {
            if (slotIndex >= InventoryPanel.transform.childCount)
            {
                Debug.LogWarning("[InventoryUI] No slots left to draw items.");
                break;
            }

            var invSlot = items[i];
            var slotTransform = InventoryPanel.transform.GetChild(slotIndex);
            var slot = slotTransform.GetComponent<UnequipSlot>();
            if (!slot)
            {
                Debug.LogWarning($"[InventoryUI] Slot {slotIndex} missing Slot component.");
                slotIndex++;
                i--; // retry same item on next child
                continue;
            }

            // Prefab must be a UI GameObject with RectTransform (NOT a Canvas)
            InventoryItem item = invSlot.Item;
            if (!item.InventoryIconPrefab)
            {
                Debug.LogWarning($"[InventoryUI] Item '{invSlot.Item.ItemName}' has no InventoryIconPrefab.");
                slotIndex++;
                continue;
            }

            var newUIItem = Instantiate(item.InventoryIconPrefab, slotTransform);
            slot.currentItem = item;

            // ✅ set quantity text if exists
            var qtyText = newUIItem.GetComponentInChildren<TMP_Text>();
            if (qtyText != null)
            {
                qtyText.text = invSlot.Quantity > 1 ? invSlot.Quantity.ToString() : "";
            }
            if (qtyText == null)
                Debug.LogWarning($"[InventoryUI] No TMP_Text found in '{item.InventoryIconPrefab.name}'");

            // Make sure it fits the slot visually
            var rect = newUIItem.GetComponent<RectTransform>();
            if (!rect)
            {
                Debug.LogWarning($"[InventoryUI] Icon prefab '{item.InventoryIconPrefab.name}' has no RectTransform (is it a UI prefab?).");
            }
            else
            {
                // Fill the slot
                rect.anchorMin = Vector2.zero;
                rect.anchorMax = Vector2.one;
                rect.offsetMin = Vector2.zero;
                rect.offsetMax = Vector2.zero;
                rect.localScale = Vector3.one;
                rect.anchoredPosition = Vector2.zero;
            }

            // Optional: ensure it has an Image
            var img = newUIItem.GetComponentInChildren<Image>();
            if (!img)
                Debug.LogWarning($"[InventoryUI] Icon prefab '{item.InventoryIconPrefab.name}' has no Image component.");

            slotIndex++;
        }

        // Rebuild layout so things snap into place
        var panelRect = InventoryPanel.GetComponent<RectTransform>();
        if (panelRect)
            LayoutRebuilder.ForceRebuildLayoutImmediate(panelRect);

        Debug.Log("[InventoryUI] RefreshUI done");
    }


}
