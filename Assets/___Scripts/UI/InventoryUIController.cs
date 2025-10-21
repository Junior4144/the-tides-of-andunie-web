using UnityEngine;
using UnityEngine.UI; // 👈 make sure this line is at the top

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
}
