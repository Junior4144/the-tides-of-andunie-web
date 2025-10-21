using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

public class RewardPageController : MonoBehaviour
{
    [Header("UI References")]
    public GameObject rewardOptionPrefab;
    public Transform rewardContainer;

    [Header("Game References")]
    public RewardManager rewardManager;
    public ClaimOverlayController claimOverlay;
    public InventoryController inventoryController; // assign in inspector or found dynamically

    public void DisplayRewards(List<GameObject> rewardPrefabs)
    {
        foreach (Transform child in rewardContainer)
            Destroy(child.gameObject);

        foreach (GameObject prefab in rewardPrefabs)
        {
            Item item = prefab.GetComponent<Item>();
            GameObject option = Instantiate(rewardOptionPrefab, rewardContainer);

            // Set icon if present
            var iconTransform = option.transform.Find("Icon");
            if (iconTransform != null)
            {
                Image img = iconTransform.GetComponent<Image>();
                if (img != null && item != null && item.icon != null)
                    img.sprite = item.icon;
            }

            Button btn = option.GetComponent<Button>();
            if (btn != null && item != null)
            {
                // ✅ Pass the GameObject itself into the listener
                btn.onClick.AddListener(() => OnRewardSelected(prefab));
            }
        }
    }

    // ✅ Parameter is now a GameObject, not an Item
    private void OnRewardSelected(GameObject itemObject)
    {
        Item item = itemObject.GetComponent<Item>();
        Debug.Log($"🎁 Claimed: {item.itemName}");

        if (!inventoryController.InventoryPanel.activeInHierarchy)
        {
            inventoryController.InventoryPanel.SetActive(true);
        }

        // Find or temporarily activate the inventory controller
        if (inventoryController == null)
            inventoryController = FindFirstObjectByType<InventoryController>();

        if (inventoryController != null)
        {
            // If InventoryPage is inactive, temporarily activate it so AddItem() works
            if (!inventoryController.gameObject.activeInHierarchy)
            {
                Debug.Log("[RewardPageController] Activating InventoryPage temporarily...");
                inventoryController.gameObject.SetActive(true);
            }

            bool added = inventoryController.AddItem(itemObject);
            if (!added)
                Debug.LogWarning("⚠️ Inventory full, could not add item.");
        }
        else
        {
            Debug.LogWarning("⚠️ InventoryController not found in scene!");
        }

        // Show the overlay (now safe because it's under UI Canvas)
        if (claimOverlay != null)
        {
            claimOverlay.ShowMessage(item.itemName, item.icon);
        }

        // Hide rewards page after a short delay
        StartCoroutine(HideRewardsAfterDelay(2f));
    }


    private IEnumerator HideRewardsAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (rewardManager != null)
            rewardManager.HideRewards();
    }
}
