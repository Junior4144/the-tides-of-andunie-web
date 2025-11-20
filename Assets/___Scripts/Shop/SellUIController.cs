using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SellUIController : MonoBehaviour
{
    [Header("UI References")]
    public GameObject InventoryPanel;

    [Header("Prefabs")]
    public GameObject sellSlotPrefab;   // Slot used strictly for selling

    [Header("Settings")]
    public int slotCount = 16;

    private void Awake()
    {
        InitializeSlots();
    }

    private void OnEnable()
    {
        InventoryManager.OnInventoryChanged += RefreshUI;
        RefreshUI();
    }

    private void OnDisable()
    {
        InventoryManager.OnInventoryChanged -= RefreshUI;
    }

    // -----------------------------------------------------------
    // INITIAL SLOT CREATION
    // -----------------------------------------------------------
    private void InitializeSlots()
    {
        // Prevent double-building
        if (InventoryPanel.transform.childCount > 0) return;

        for (int i = 0; i < slotCount; i++)
            CreateSellSlot();

        LayoutRebuilder.ForceRebuildLayoutImmediate(
            InventoryPanel.GetComponent<RectTransform>()
        );
    }

    private void CreateSellSlot()
    {
        var slotObj = Instantiate(sellSlotPrefab, InventoryPanel.transform);

        // Must always have a SellSlot
        if (!slotObj.GetComponent<SellSlot>())
        {
            Debug.LogError("[SellUI] sellSlotPrefab MUST contain a SellSlot component.");
        }
    }

    // -----------------------------------------------------------
    // UI REFRESH
    // -----------------------------------------------------------
    private void RefreshUI()
    {
        if (!InventoryPanel) return;

        Debug.Log("[SellUI] RefreshUI");

        // ---------- CLEAR ALL SLOTS ----------
        foreach (Transform slotTransform in InventoryPanel.transform)
        {
            // Clear visuals
            for (int i = slotTransform.childCount - 1; i >= 0; i--)
                Destroy(slotTransform.GetChild(i).gameObject);

            // Clear data reference
            var sellSlot = slotTransform.GetComponent<SellSlot>();
            if (sellSlot) sellSlot.currentItem = null;
        }

        // ---------- DRAW INVENTORY ITEMS ----------
        var items = InventoryManager.Instance.GetAllItems();
        if (items == null)
        {
            Debug.LogError("[SellUI] InventoryManager returned NULL items list.");
            return;
        }

        int index = 0;
        foreach (var entry in items)
        {
            if (index >= InventoryPanel.transform.childCount) break;

            var slotTransform = InventoryPanel.transform.GetChild(index);
            var sellSlot = slotTransform.GetComponent<SellSlot>();

            IInventoryItem item = entry.Item;

            // Create icon UI
            var newIcon = Instantiate(item.InventoryIconPrefab, slotTransform);

            // Anchor fill
            var rect = newIcon.GetComponent<RectTransform>();
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;
            rect.localScale = Vector3.one; // <--- ADD THIS
            // Set quantity text
            var qtyText = newIcon.GetComponentInChildren<TMP_Text>();
            if (qtyText != null)
                qtyText.text = entry.Quantity > 1 ? entry.Quantity.ToString() : "";

            // Assign the actual item
            sellSlot.currentItem = item;

            index++;
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(
            InventoryPanel.GetComponent<RectTransform>()
        );
    }
}
