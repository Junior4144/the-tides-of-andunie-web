using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[RequireComponent(typeof(AudioSource))]
public class RaidRewardManager : MonoBehaviour
{
    public static RaidRewardManager Instance;
    [SerializeField] public RaidController RaidController;
    [SerializeField] public RewardUIController RewardUI;
    [SerializeField] private Transform RewardContainer;
    [SerializeField] private GameObject RewardItemUIPrefab;
    [SerializeField] private float showRewardsDelay = 0.5f;
    [SerializeField] private AudioClip rewardSound;
    public static event Action OnRewardCollected;

    private AudioSource _audioSource;

    void Awake()
    {
        Instance = this;
        _audioSource = GetComponent<AudioSource>();
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

        StartCoroutine(ShowRewardsAfterDelay(rewards));
    }

    public void ReportRewardCollected()
    {
        OnRewardCollected?.Invoke();
    }

    private IEnumerator ShowRewardsAfterDelay(List<RewardListing> rewards)
    {
        yield return new WaitForSeconds(showRewardsDelay);
        PlayRewardSound();
        yield return new WaitForSeconds(0.1f);
        RewardUI.ShowRewards(rewards);
    }

    private void PlayRewardSound()
    {
        if (rewardSound != null)
            _audioSource.PlayOneShot(rewardSound);
        else
            Debug.LogWarning("[RaidRewardManager] Reward sound is null. Playing no sound");
    }
}