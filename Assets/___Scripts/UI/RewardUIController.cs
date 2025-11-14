using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using DG.Tweening;

public class RewardUIController : MonoBehaviour
{
    [SerializeField] private GameObject _rewardsCanvas;
    [SerializeField] private GameObject _rewardsPage;
    [SerializeField] private Transform _rewardContainer;
    [SerializeField] private GameObject _rewardItemUIPrefab;
    [SerializeField] private float hideDelay = 0.35f;

    void Start()
    {
        _rewardsCanvas.SetActive(false);
    }

    public void ShowRewards(List<RewardListing> rewardsToShow)
    {
        if (rewardsToShow == null || rewardsToShow.Count == 0)
        {
            Debug.LogWarning("ShowRewards called, but no rewards were provided.");
            return;
        }

        ClearExistingRewards();
        InstantiateRewardUI(rewardsToShow);
        ActivateRewardsCanvas();
    }

    public void HideRewards()
    {
        UIEvents.OnRewardDeactivated?.Invoke();
        StartCoroutine(HideRewardsAfterDelay());
    }

    private void ClearExistingRewards()
    {
        var existingRewards = _rewardContainer.Cast<Transform>().ToList();
        existingRewards.ForEach(child => Destroy(child.gameObject));
    }

    private void InstantiateRewardUI(List<RewardListing> rewards)
    {
        rewards.ForEach(reward =>
        {
            var uiObject = Instantiate(_rewardItemUIPrefab, _rewardContainer);
            var rewardUI = uiObject.GetComponent<RewardItemUI>();
            rewardUI.SetData(reward, this);
        });
    }

    private void ActivateRewardsCanvas()
    {
        UIEvents.OnRewardActive.Invoke();
        _rewardsCanvas.SetActive(true);
    }

    private IEnumerator HideRewardsAfterDelay()
    {
        RectTransform rt = _rewardsPage.GetComponent<RectTransform>();
        rt.DOScale(0f, hideDelay).SetEase(Ease.InBack);

        yield return new WaitForSeconds(hideDelay);

        _rewardsCanvas.SetActive(false);
    }
}
