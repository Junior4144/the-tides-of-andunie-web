using UnityEngine;
using System.Collections.Generic;

public class RaidRewardManager : MonoBehaviour
{
    public static RaidRewardManager Instance;
    [SerializeField] public RaidController raidController;
    [SerializeField] public RewardController rewardController;

    [SerializeField] private Transform shopPanel;
    [SerializeField] private GameObject shopItemUIPrefab;
    [SerializeField] public GameObject canvas;


    void Awake()
    {
        Instance = this;
        rewardController.HideRewards();
    }

    public void OnEnable()
    {
        raidController.OnRaidComplete += HandleRaidFinished;
    }

    public void OnDisable()
    {
        raidController.OnRaidComplete -= HandleRaidFinished;
    }

    private void HandleRaidFinished()
    {
        List<RewardListing> rewards = raidController.RaidCompletionRewards;

        foreach (var reward in rewards)
        {
            var uiObj = Instantiate(shopItemUIPrefab, shopPanel);
            var ui = uiObj.GetComponent<ShopItemUI>();

            ui.SetData(reward);
        }

        rewardController.ShowRewards(rewards);
    }
}