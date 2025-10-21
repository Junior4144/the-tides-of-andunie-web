using UnityEngine;
using UnityEngine.UI;

public class InventoryUIController : MonoBehaviour
{
    public GameObject InventoryPanel;
    public GameObject slotPrefab;
    public int slotCount;
    public GameObject[] itemPrefabs;

    public static InventoryUIController Instance;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        InitializeSlots();
    }

    void OnEnable()
    {
        InventoryManager.OnInventoryChanged += RefreshUI;
        RefreshUI(); // <<-- add this so UI updates immediately when opening
        //InitializeSlots();
    }

    void OnDisable()
    {
        InventoryManager.OnInventoryChanged -= RefreshUI;
    }

    private void InitializeSlots()
    {
        // Prevent double-creation if already filled
        if (InventoryPanel.transform.childCount > 0) return;

        for (int i = 0; i < slotCount; i++)
        {
            Slot slot = Instantiate(slotPrefab, InventoryPanel.transform).GetComponent<Slot>();
            if (i < itemPrefabs.Length)
            {
                GameObject item = Instantiate(itemPrefabs[i], slot.transform);
                RectTransform rect = item.GetComponent<RectTransform>();
                rect.anchoredPosition = Vector2.zero;
                rect.localScale = Vector3.one;
                slot.currentItem = item;
            }
        }

        // ✅ Force Unity to rebuild the layout once all slots exist
        LayoutRebuilder.ForceRebuildLayoutImmediate(InventoryPanel.GetComponent<RectTransform>());
        Debug.Log($"[InventoryController] Initialized {slotCount} slots.");
    }

    public bool AddItem(GameObject itemPrefab)
    {
        Debug.Log($"[InventoryController] Checking {InventoryPanel.transform.childCount} slots...");

        int index = 0;
        foreach (Transform slotTransform in InventoryPanel.transform)
        {
            Slot slot = slotTransform.GetComponent<Slot>();
            if (slot == null)
            {
                Debug.LogWarning($"[InventoryController] Child {index}: No Slot component!");
            }
            else
            {
                Debug.Log($"[InventoryController] Slot {index}: currentItem = {(slot.currentItem ? slot.currentItem.name : "null")}");
                if (slot.currentItem == null)
                {
                    GameObject newItem = Instantiate(itemPrefab, slotTransform);
                    RectTransform rect = newItem.GetComponent<RectTransform>();
                    rect.anchoredPosition = Vector2.zero;
                    rect.localScale = Vector3.one;
                    slot.currentItem = newItem;

                    // ✅ Force layout rebuild right after adding an item
                    LayoutRebuilder.ForceRebuildLayoutImmediate(InventoryPanel.GetComponent<RectTransform>());

                    Debug.Log($"[InventoryController] ✅ Added {newItem.name} to Slot {index}");
                    return true;
                }
            }
            index++;
        }

        Debug.LogWarning("⚠️ Inventory full, could not add item.");
        return false;
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
            var slot = slotTransform.GetComponent<Slot>();
            if (!slot)
            {
                Debug.LogWarning($"[InventoryUI] Child '{slotTransform.name}' has no Slot component.");
                continue;
            }

            if (slot.currentItem)
            {
                Destroy(slot.currentItem);
                slot.currentItem = null;
                cleared++;
            }
        }
        Debug.Log($"[InventoryUI] Cleared {cleared} items");

        // 2) Rebuild from inventory data
        var items = InventoryManager.Instance.GetAllItems();
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
            var slot = slotTransform.GetComponent<Slot>();
            if (!slot)
            {
                Debug.LogWarning($"[InventoryUI] Slot {slotIndex} missing Slot component.");
                slotIndex++;
                i--; // retry same item on next child
                continue;
            }

            // Prefab must be a UI GameObject with RectTransform (NOT a Canvas)
            GameObject prefab = invSlot.Item.InventoryIconPrefab;
            if (!prefab)
            {
                Debug.LogWarning($"[InventoryUI] Item '{invSlot.Item.ItemName}' has no InventoryIconPrefab.");
                slotIndex++;
                continue;
            }

            var newUIItem = Instantiate(prefab, slotTransform);
            slot.currentItem = newUIItem;

            // Make sure it fits the slot visually
            var rect = newUIItem.GetComponent<RectTransform>();
            if (!rect)
            {
                Debug.LogWarning($"[InventoryUI] Icon prefab '{prefab.name}' has no RectTransform (is it a UI prefab?).");
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
                Debug.LogWarning($"[InventoryUI] Icon prefab '{prefab.name}' has no Image component.");

            slotIndex++;
        }

        // Rebuild layout so things snap into place
        var panelRect = InventoryPanel.GetComponent<RectTransform>();
        if (panelRect)
            LayoutRebuilder.ForceRebuildLayoutImmediate(panelRect);

        Debug.Log("[InventoryUI] RefreshUI done");
    }
}
