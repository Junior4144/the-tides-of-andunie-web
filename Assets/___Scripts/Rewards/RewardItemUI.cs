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
    [SerializeField] private AudioClip clickSound;
    [SerializeField] private AudioClip errorSound;

    private RewardListing reward_listing;
    private RewardUIController rewardUIController;
    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }
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
        if (rewardUIController.IsConfirmationActive)
            return;

        if (InventoryManager.Instance.AddItem(reward_listing.Item))
        {
            PlaySound(clickSound);
            rewardUIController.HideRewards();
            RaidRewardManager.Instance.ReportRewardCollected();
            return;
        }
        PlaySound(errorSound);
        rewardUIController.ShowLimitReachedConfirmation(OnAcceptCoinValue);
    }

    private void PlaySound(AudioClip clip)
    {
        if (clip != null)
            _audioSource.PlayOneShot(clip);
    }

    private void OnAcceptCoinValue()
    {
        int coinValue = GetCoinValue();
        CurrencyManager.Instance.AddCoins(coinValue);
        rewardUIController.HideRewards();
        RaidRewardManager.Instance.ReportRewardCollected();
    }

    private int GetCoinValue()
    {
        return reward_listing.Item.SellAmount;
    }

    public void ResetErrors()
    {
        ErrorText.gameObject.SetActive(false);
    }
}
