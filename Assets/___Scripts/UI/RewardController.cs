using UnityEngine;
using System.Collections.Generic;
using System.Collections;
public class RewardController : MonoBehaviour
{
    public GameObject rewardsCanvas;

    void Start()
    {
        rewardsCanvas.SetActive(false);
    }
    public void ShowRewards(List<RewardListing> rewardsToShow)
    {
        // If list is empty, do nothing
        if (rewardsToShow == null || rewardsToShow.Count == 0)
        {
            Debug.LogWarning("ShowRewards called, but no rewards were provided.");
            return;
        }
        
        rewardsCanvas.SetActive(true);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            rewardsCanvas.SetActive(!rewardsCanvas.activeSelf);
            Debug.Log("key pressed");
        }
    }

    public void HideRewards()
    {
        rewardsCanvas.SetActive(false);
    }
}
