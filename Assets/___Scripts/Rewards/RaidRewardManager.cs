using UnityEngine;
using System.Collections.Generic;

public class RaidRewardManager : MonoBehaviour
{
    public static RaidRewardManager Instance;
    [SerializeField] public RaidController RaidController;
    [SerializeField] public RewardUIController RewardUI;
    [SerializeField] private Transform RewardContainer;
    [SerializeField] private GameObject RewardItemUIPrefab;


    void Awake()
    {
        Instance = this;
        RewardUI.HideRewards();
    }

    public void OnEnable()
    {
        RaidController.OnRaidComplete += HandleRaidFinished;
    }

    public void OnDisable()
    {
        RaidController.OnRaidComplete -= HandleRaidFinished;
    }

    private void HandleRaidFinished()
    {
        List<RewardListing> rewards = RaidController.RaidCompletionRewards;

        foreach (var reward in rewards)
        {
            var uiObj = Instantiate(RewardItemUIPrefab, RewardContainer);
            var ui = uiObj.GetComponent<RewardItemUI>();
            ui.SetData(reward);
        }

        RewardUI.ShowRewards(rewards);
    }
}