using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DyanmicToolTips : MonoBehaviour
{
    [SerializeField] private RectTransform toolTipsRoot;
    [SerializeField] private Vector2 offset = new(40f, -40f);
    [SerializeField] private GameObject iconPanel;
    [SerializeField] private Transform statsPanel;
    [SerializeField] private RectTransform mainPanel;

    [Header("Tooltip Configuration")]
    [SerializeField] private TMP_FontAsset tooltipFont;
    [SerializeField] private float fontSize = 18f;
    [SerializeField] private float rowHeight = 30f;
    [SerializeField] private float minPanelWidth = 200f;
    [SerializeField] private float mainPanelPadding = 20f;

    private readonly List<GameObject> spawnedEffectRows = new();
    private ItemEffect[] lastEffects;
    public ShopListing shopListing;
    private Image iconImage;
    private Sprite lastSprite;


    private void Awake()
    {
        iconImage = iconPanel.GetComponent<Image>();
    }

    private void OnEnable()
    {
        Vector2 mousePos = Input.mousePosition;
        toolTipsRoot.position = mousePos + offset;

        UnityEngine.Canvas.ForceUpdateCanvases();   // Force UI to update THIS frame
    }

    private void Update()
    {
        UpdatePosition();
        UpdateIconIfChanged();
        UpdateStatsIfChanged();
    }

    private void LateUpdate()
    {
        Vector2 mousePos = Input.mousePosition;
        toolTipsRoot.position = mousePos + offset;
    }

    void UpdatePosition()
    {
        Vector2 mousePos = Input.mousePosition;
        toolTipsRoot.position = mousePos + offset;
    }

    void UpdateIconIfChanged()
    {
        if (shopListing?.Item == null)
            return;

        if (lastSprite != shopListing.Item.SpriteIcon)
        {
            lastSprite = shopListing.Item.SpriteIcon;
            iconImage.sprite = lastSprite;
        }
    }

    private void UpdateStatsIfChanged()
    {
        if (shopListing?.Item == null)
            return;

        ItemEffect[] effects = shopListing.Item.GetEffects();

        if (lastEffects == effects)
            return;

        lastEffects = effects;

        ClearStatRows();

        if (effects != null && effects.Length > 0)
        {
            foreach (var effect in effects)
                CreateStatRow(effect);
        }

        CreateStackSizeRow();
        CreateSellPriceRow();

        AdjustPanelWidth();

        UnityEngine.Canvas.ForceUpdateCanvases();
    }

    private void ClearStatRows()
    {
        foreach (var row in spawnedEffectRows)
            Destroy(row);

        spawnedEffectRows.Clear();
    }

    private void CreateStatRow(ItemEffect effect)
    {
        GameObject textObject = new($"StatRow_{effect}");
        textObject.transform.SetParent(statsPanel, false);

        RectTransform rectTransform = textObject.AddComponent<RectTransform>();
        TextMeshProUGUI text = textObject.AddComponent<TextMeshProUGUI>();

        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, rowHeight);

        text.text = effect.ToString();
        text.font = tooltipFont;
        text.fontSize = fontSize;
        text.alignment = TextAlignmentOptions.Left;
        text.color = effect.ItemAmount >= 0 ? Color.green : Color.red;
        text.textWrappingMode = TextWrappingModes.NoWrap;
        text.overflowMode = TextOverflowModes.Overflow;

        spawnedEffectRows.Add(textObject);
    }

    private void CreateStackSizeRow()
    {
        if (shopListing?.Item == null)
            return;

        GameObject textObject = new("StatRow_StackSize");
        textObject.transform.SetParent(statsPanel, false);

        RectTransform rectTransform = textObject.AddComponent<RectTransform>();
        TextMeshProUGUI text = textObject.AddComponent<TextMeshProUGUI>();

        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, rowHeight);

        text.text = $"Stack Size: {shopListing.Item.MaxStackSize}";
        text.font = tooltipFont;
        text.fontSize = fontSize;
        text.alignment = TextAlignmentOptions.Left;
        text.color = Color.black;
        text.textWrappingMode = TextWrappingModes.NoWrap;
        text.overflowMode = TextOverflowModes.Overflow;

        spawnedEffectRows.Add(textObject);
    }

    private void CreateSellPriceRow()
    {
        if (shopListing?.Item == null)
            return;

        GameObject textObject = new("StatRow_SellPrice");
        textObject.transform.SetParent(statsPanel, false);

        RectTransform rectTransform = textObject.AddComponent<RectTransform>();
        TextMeshProUGUI text = textObject.AddComponent<TextMeshProUGUI>();

        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, rowHeight);

        text.text = $"Sell Price: {shopListing.Item.SellAmount}";
        text.font = tooltipFont;
        text.fontSize = fontSize;
        text.alignment = TextAlignmentOptions.Left;
        text.color = Color.black;
        text.textWrappingMode = TextWrappingModes.NoWrap;
        text.overflowMode = TextOverflowModes.Overflow;

        spawnedEffectRows.Add(textObject);
    }

    private void AdjustPanelWidth()
    {
        if (mainPanel == null || !spawnedEffectRows.Any())
            return;

        var requiredWidth = CalculateRequiredWidth();
        SetPanelWidth(requiredWidth);
    }

    private float CalculateRequiredWidth() => 
        Mathf.Max(minPanelWidth, GetIconWidth() + GetMaxTextWidth() + mainPanelPadding);

    private float GetIconWidth()
    {
        if (iconPanel == null)
            return 0f;

        RectTransform iconRect = iconPanel.GetComponent<RectTransform>();
        return iconRect != null ? iconRect.sizeDelta.x : 0f;
    }
        

    private float GetMaxTextWidth() =>
        spawnedEffectRows
            .Select(GetTextComponent)
            .Where(text => text != null)
            .Select(GetTextWidth)
            .DefaultIfEmpty(0f)
            .Max();

    private TextMeshProUGUI GetTextComponent(GameObject row) =>
        row.GetComponent<TextMeshProUGUI>();

    private float GetTextWidth(TextMeshProUGUI text)
    {
        text.ForceMeshUpdate();

        return text.preferredWidth;
    }

    private void SetPanelWidth(float width) =>
        mainPanel.sizeDelta = new Vector2(width, mainPanel.sizeDelta.y);
}