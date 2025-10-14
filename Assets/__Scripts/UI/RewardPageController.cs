using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class RewardPageController : MonoBehaviour
{
    [Header("UI References")]
    public GameObject rewardOptionPrefab;  // Prefab with Image (+ optional Button)
    public Transform rewardContainer;      // Empty GameObject under RewardsPage
    [Header("Game References")]
    public InventoryController inventoryController;
    public RewardManager rewardManager;

    public void DisplayRewards(List<GameObject> rewardPrefabs)
    {

        foreach (Transform child in rewardContainer)
            Destroy(child.gameObject);

        foreach (GameObject prefab in rewardPrefabs)
        {

            Item item = prefab.GetComponent<Item>();

            GameObject option = Instantiate(rewardOptionPrefab, rewardContainer);

            // Look for Icon
            var iconTransform = option.transform.Find("Icon");
            if (iconTransform == null)
            {
                Debug.LogWarning("⚠️ RewardOption prefab missing child 'Icon'");
            }
            else
            {
                Image img = iconTransform.GetComponent<Image>();
                if (img != null && item != null && item.icon != null)
                {
                    img.sprite = item.icon;
                    img.color = Color.white;
                    Debug.Log($"🖼️  Set icon for {item.itemName}");
                }
                else
                {
                    Debug.LogWarning("⚠️ Icon not assigned or Item.icon missing");
                }
            }

            Button btn = option.GetComponent<Button>();
            if (btn != null)
            {
                // Capture local variable to avoid closure issue
                GameObject capturedPrefab = prefab;

                btn.onClick.AddListener(() =>
                {
                    OnRewardSelected();

                    Debug.Log("Clicked button!");
Debug.Log("RewardManager ref: " + rewardManager);

                });
            }
        }

    }
    
    private void OnRewardSelected()
    {
        if (rewardManager != null)
        {
            rewardManager.HideRewards();
            Debug.Log("Yo mama");
            Debug.Log("Clicked button!");
            Debug.Log("RewardManager ref: " + rewardManager);

        }
    }
}

