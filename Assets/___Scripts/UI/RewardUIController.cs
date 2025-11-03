using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using DG.Tweening;

public class RewardUIController : MonoBehaviour
{
    public GameObject rewardsCanvas;
    public GameObject rewardsPage;
    [SerializeField] private float hideDelay = 0.35f;

    void Start()
    {
        rewardsCanvas.SetActive(false);
    }

    public void ShowRewards(List<RewardListing> rewardsToShow)
    {
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
            if (rewardsCanvas.activeSelf)
                HideRewards();
            else
                rewardsCanvas.SetActive(true);
        }
    }

    public void HideRewards()
    {
        StartCoroutine(HideRewardsAfterDelay());
    }

    private IEnumerator HideRewardsAfterDelay()
    {
        RectTransform rt = rewardsPage.GetComponent<RectTransform>();
        rt.DOScale(0f, hideDelay).SetEase(Ease.InBack);

        yield return new WaitForSeconds(hideDelay);

        rewardsCanvas.SetActive(false);
    }
}
