using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RewardItemUI : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private Button buyButton;
    [SerializeField] private TMP_Text ErrorText;

    private RewardListing reward_listing;
    private RewardUIController rewardUIController;

    private void Start() => ErrorText.gameObject.SetActive(false);

    private void OnDisable() => ErrorText.gameObject.SetActive(false);

    public void SetData(RewardListing reward, RewardUIController controller)
    {
        reward_listing = reward;
        rewardUIController = controller;

        nameText.text = reward_listing.Item.ItemName;
        icon.sprite = reward_listing.Item.InventoryIconPrefab.GetComponentInChildren<Image>().sprite;

        buyButton.onClick.AddListener(HandleRewardClick);
    }

    void HandleRewardClick()
    {
        InventoryManager.Instance.AddItem(reward_listing.Item);
        rewardUIController.HideRewards();
        RaidRewardManager.Instance.ReportRewardCollected();
    }

    private void HandleLimitReached() { StopAllCoroutines(); StartCoroutine(LimitReached()); }

    private IEnumerator LimitReached()
    {
        ErrorText.text = "Limit Reached";
        ErrorText.gameObject.SetActive(true);

        yield return new WaitForSeconds(1f);

        ErrorText.gameObject.SetActive(false);
    }

    public void ResetErrors()
    {
        ErrorText.gameObject.SetActive(false);
    }
}
