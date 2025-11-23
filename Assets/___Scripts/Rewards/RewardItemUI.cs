using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RewardItemUI : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private Transform effectsContainer;
    [SerializeField] private TMP_FontAsset effectFont;
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

        SetItemName();
        SetItemIcon();
        SetItemEffects();

        buyButton.onClick.AddListener(HandleRewardClick);
    }

    private void SetItemName() =>
        nameText.text = reward_listing.Item.ItemName.Replace(" ", "   ");

    private void SetItemIcon()
    {
        icon.sprite = reward_listing.Item.SpriteIcon;
        icon.rectTransform.sizeDelta = new Vector2(200, 200);
        icon.preserveAspect = true;
    }

    private void SetItemEffects()
    {
        ClearExistingEffects();

        var effects = reward_listing.Item.GetEffects();
        if (effects == null || effects.Length == 0) return;

        var sortedEffects = SortEffectsBySign(effects.Where(effect => effect != null).ToArray());
        sortedEffects.ForEach(CreateEffectText);
    }

    private void ClearExistingEffects()
    {
        var existingEffects = effectsContainer.Cast<Transform>().ToList();
        existingEffects.ForEach(child => Destroy(child.gameObject));
    }

    private System.Collections.Generic.List<ItemEffect> SortEffectsBySign(ItemEffect[] effects)
    {
        var positiveEffects = effects.Where(IsPositiveEffect).ToList();
        var negativeEffects = effects.Where(effect => !IsPositiveEffect(effect)).ToList();

        positiveEffects.AddRange(negativeEffects);
        return positiveEffects;
    }

    private bool IsPositiveEffect(ItemEffect effect) =>
        effect.ToString().StartsWith("+");

    private void CreateEffectText(ItemEffect effect)
    {
        var textObject = new GameObject("EffectText");
        textObject.transform.SetParent(effectsContainer, false);

        var rectTransform = textObject.AddComponent<RectTransform>();
        SetTextHeight(rectTransform);

        var textComponent = textObject.AddComponent<TextMeshProUGUI>();
        ConfigureTextComponent(textComponent, effect);
    }

    private void SetTextHeight(RectTransform rectTransform)
    {
        rectTransform.sizeDelta = new Vector2(0, 25);
    }

    private void ConfigureTextComponent(TextMeshProUGUI textComponent, ItemEffect effect)
    {
        textComponent.text = effect.ToString();
        textComponent.font = effectFont;
        textComponent.fontSize = 18;
        textComponent.alignment = TextAlignmentOptions.Left;
        textComponent.color = GetEffectColor(effect);
    }

    private Color GetEffectColor(ItemEffect effect) =>
        IsPositiveEffect(effect) ? Color.green : Color.red;

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
