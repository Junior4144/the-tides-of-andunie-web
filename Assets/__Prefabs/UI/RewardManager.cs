using UnityEngine;
using System.Collections.Generic;
using System.Collections;
public class RewardManager : MonoBehaviour
{
    // [Header("References")]
    public GameObject rewardsCanvas;                // assigned in Inspector
    public RewardPageController rewardPageController; // found at runtime
    public List<GameObject> rewardsList;

    void Start()
    {
        rewardPageController = GetComponentInChildren<RewardPageController>();
        if (rewardsCanvas != null)
            rewardsCanvas.SetActive(false);
        rewardPageController.DisplayRewards(rewardsList);
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            rewardsCanvas.SetActive(!rewardsCanvas.activeSelf);

        }
    }
    
    public void HideRewards()
    {
        rewardsCanvas.SetActive(false);

    }
}
