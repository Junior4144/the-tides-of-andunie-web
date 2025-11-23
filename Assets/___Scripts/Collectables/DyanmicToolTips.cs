using System.Collections.Generic;
using System.Reflection;
using TMPro;
using Unity.AppUI.UI;
using UnityEngine;
using UnityEngine.UI;

public class DyanmicToolTips : MonoBehaviour
{
    [SerializeField] private RectTransform toolTipsRoot;
    [SerializeField] private Vector2 offset = new(40f, -40f);
    [SerializeField] private GameObject iconPanel;
    [SerializeField] private Transform statsPanel;
    [SerializeField] private GameObject effectRowPrefab;

    private readonly List<GameObject> spawnedEffectRows = new();
    private ItemEffect[] lastEffects;

    public ShopListing shopListing;
    private Image iconImage;

    private Sprite lastSprite;

    private void Awake()
    {
        iconImage = iconPanel.GetComponent<Image>();
    }

    private void Update()
    {
        UpdatePosition();
        UpdateIconIfChanged();
        UpdateStatsIfChanged();
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

        // If nothing changed, do nothing
        if (lastEffects == effects)
            return;

        lastEffects = effects;

        // Clear old UI rows
        foreach (var row in spawnedEffectRows)
            Destroy(row);

        spawnedEffectRows.Clear();

        if (effects == null || effects.Length == 0)
            return;

        // Build new UI rows
        foreach (var effect in effects)
        {
            GameObject row = Instantiate(effectRowPrefab, statsPanel);
            TextMeshProUGUI text = row.GetComponentInChildren<TextMeshProUGUI>();

            text.text = effect.ToString();       // Uses your formatting
            text.color = effect.ItemAmount >= 0 ? Color.green : Color.red;

            spawnedEffectRows.Add(row);
        }
    }
}