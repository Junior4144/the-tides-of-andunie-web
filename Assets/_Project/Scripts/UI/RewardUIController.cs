using UnityEngine;
using UnityEngine.UI;
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

    [Header("Confirmation Popup")]
    [SerializeField] private GameObject confirmationPanel;
    [SerializeField] private Button yesButton;
    [SerializeField] private Button noButton;
    [SerializeField] private AudioClip confirmSound;

    private System.Action onConfirmCallback;
    private AudioSource audioSource;
    public bool IsConfirmationActive { get; private set; }

    void Start()
    {
        _rewardsCanvas.SetActive(false);
        audioSource = GetComponent<AudioSource>();
        SetupConfirmationButtons();
    }

    private void SetupConfirmationButtons()
    {
        if (yesButton != null)
            yesButton.onClick.AddListener(OnConfirmYes);

        if (noButton != null)
            noButton.onClick.AddListener(OnConfirmNo);
    }

    public void ShowRewards(List<RewardListing> rewardsToShow)
    {
        if (rewardsToShow == null || rewardsToShow.Count == 0)
        {
            Debug.LogWarning("ShowRewards called, but no rewards were provided.");
            return;
        }

        PlayerManager.Instance.DisablePlayerMovement();

        HideConfirmation();
        ClearExistingRewards();
        InstantiateRewardUI(rewardsToShow);
        ActivateRewardsCanvas();
    }

    public void HideRewards()
    {
        PlayerManager.Instance.EnablePlayerMovement();
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

    public void ShowLimitReachedConfirmation(System.Action onAccept)
    {
        IsConfirmationActive = true;
        onConfirmCallback = onAccept;
        confirmationPanel.SetActive(true);
    }

    public void OnConfirmYes()
    {
        PlayConfirmSound();
        var callback = onConfirmCallback;
        HideConfirmation();
        DOVirtual.DelayedCall(0.2f, () => callback?.Invoke());
    }

    private void PlayConfirmSound()
    {
        if (audioSource != null && confirmSound != null)
            audioSource.PlayOneShot(confirmSound);
    }

    public void OnConfirmNo()
    {
        PlayConfirmSound();
        HideConfirmation();
    }

    private void HideConfirmation()
    {
        IsConfirmationActive = false;
        onConfirmCallback = null;
        confirmationPanel.SetActive(false);
    }
}
