using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatsUIController : MonoBehaviour
{
    [Header("Container References")]
    [SerializeField] private Transform statsContainer;
    [SerializeField] private TMP_FontAsset statFont;

    [Header("UI Configuration")]
    [SerializeField] private float statElementHeight = 50f;
    [SerializeField] private float statElementWidth = 1400f;
    [SerializeField] private float mainTextFontSize = 48f;
    [SerializeField] private float additiveTextFontSize = 46f;
    [SerializeField] private float horizontalSpacing = 10f;

    [Header("Width Ratios")]
    [SerializeField] [Range(0.3f, 0.7f)] private float labelWidthRatio = 0.5f;
    [SerializeField] [Range(0.1f, 0.4f)] private float mainValueWidthRatio = 0.15f;
    [SerializeField] [Range(0.1f, 0.4f)] private float additiveValueWidthRatio = 0.15f;

    private readonly Dictionary<StatType, StatUIElement> statElements = new();

    private class StatUIElement
    {
        public TextMeshProUGUI MainText;
        public TextMeshProUGUI AdditiveText;
        public GameObject Container;
    }

    private void Start()
    {
        GenerateAllStatElements();
    }

    private void OnEnable()
    {
        SubscribeToAllStatEvents();
        InitializeAllStats();
    }

    private void OnDisable()
    {
        UnsubscribeFromAllStatEvents();
    }

    private void SubscribeToAllStatEvents()
    {
        foreach (StatType statType in Enum.GetValues(typeof(StatType)))
        {
            string eventName = GetEventNameForStat(statType);
            SubscribeToStatEvent(statType, eventName);
        }
    }

    private void UnsubscribeFromAllStatEvents()
    {
        foreach (StatType statType in Enum.GetValues(typeof(StatType)))
        {
            string eventName = GetEventNameForStat(statType);
            UnsubscribeFromStatEvent(statType, eventName);
        }
    }

    private string GetEventNameForStat(StatType statType)
    {
        return $"On{statType}Changed";
    }

    private void SubscribeToStatEvent(StatType statType, string eventName)
    {
        EventInfo eventInfo = typeof(PlayerStatsManager).GetEvent(eventName, BindingFlags.Public | BindingFlags.Static);
        if (eventInfo != null)
        {
            Action<float, float> handler = (current, defaultVal) => UpdateStat(statType, current, defaultVal);
            Delegate delegateHandler = Delegate.CreateDelegate(eventInfo.EventHandlerType, handler.Target, handler.Method);
            eventInfo.AddEventHandler(null, delegateHandler);
        }
    }

    private void UnsubscribeFromStatEvent(StatType statType, string eventName)
    {
        EventInfo eventInfo = typeof(PlayerStatsManager).GetEvent(eventName, BindingFlags.Public | BindingFlags.Static);
        if (eventInfo != null)
        {
            Action<float, float> handler = (current, defaultVal) => UpdateStat(statType, current, defaultVal);
            Delegate delegateHandler = Delegate.CreateDelegate(eventInfo.EventHandlerType, handler.Target, handler.Method);
            eventInfo.RemoveEventHandler(null, delegateHandler);
        }
    }

    private void InitializeAllStats()
    {
        if (PlayerStatsManager.Instance == null) return;

        foreach (StatType statType in Enum.GetValues(typeof(StatType)))
        {
            float currentValue = GetStatValue(statType);
            float defaultValue = GetDefaultStatValue(statType);
            UpdateStat(statType, currentValue, defaultValue);
        }
    }

    private float GetStatValue(StatType statType)
    {
        PropertyInfo property = typeof(PlayerStatsManager).GetProperty(statType.ToString());
        return property != null ? (float)property.GetValue(PlayerStatsManager.Instance) : 0f;
    }

    private float GetDefaultStatValue(StatType statType)
    {
        PropertyInfo property = typeof(PlayerStatsManager).GetProperty($"Default{statType}");
        return property != null ? (float)property.GetValue(PlayerStatsManager.Instance) : 0f;
    }

    private void GenerateAllStatElements()
    {
        ClearExistingElements();

        foreach (StatType statType in Enum.GetValues(typeof(StatType)))
        {
            CreateStatElement(statType);
        }

        Canvas.ForceUpdateCanvases();
        LayoutRebuilder.ForceRebuildLayoutImmediate(statsContainer.GetComponent<RectTransform>());
    }

    private void ClearExistingElements()
    {
        var existingElements = statsContainer.Cast<Transform>().ToList();
        existingElements.ForEach(child => Destroy(child.gameObject));
        statElements.Clear();
    }

    private void CreateStatElement(StatType statType)
    {
        GameObject container = new GameObject($"Stat_{statType}");
        container.transform.SetParent(statsContainer, false);

        RectTransform rectTransform = container.AddComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(statElementWidth, statElementHeight);

        HorizontalLayoutGroup layoutGroup = container.AddComponent<HorizontalLayoutGroup>();
        layoutGroup.childControlWidth = true;
        layoutGroup.childControlHeight = true;
        layoutGroup.childForceExpandWidth = false;
        layoutGroup.childForceExpandHeight = false;
        layoutGroup.spacing = horizontalSpacing;
        layoutGroup.childAlignment = TextAnchor.MiddleLeft;
        layoutGroup.padding = new RectOffset(0, 0, 0, 0);

        GameObject labelObject = new("Label");
        labelObject.transform.SetParent(container.transform, false);
        TextMeshProUGUI labelText = labelObject.AddComponent<TextMeshProUGUI>();
        ConfigureLabelText(labelText, statType);

        GameObject mainValueObject = new("MainValue");
        mainValueObject.transform.SetParent(container.transform, false);
        TextMeshProUGUI mainText = mainValueObject.AddComponent<TextMeshProUGUI>();
        ConfigureMainText(mainText);

        GameObject additiveValueObject = new("AdditiveValue");
        additiveValueObject.transform.SetParent(container.transform, false);
        TextMeshProUGUI additiveText = additiveValueObject.AddComponent<TextMeshProUGUI>();
        ConfigureAdditiveText(additiveText);

        statElements[statType] = new StatUIElement
        {
            MainText = mainText,
            AdditiveText = additiveText,
            Container = container
        };
    }

    private void ConfigureLabelText(TextMeshProUGUI textComponent, StatType statType)
    {
        string displayName = GetStatDisplayName(statType);

        float labelWidth = statElementWidth * labelWidthRatio;

        LayoutElement layoutElement = textComponent.gameObject.AddComponent<LayoutElement>();
        layoutElement.minWidth = labelWidth;
        layoutElement.preferredWidth = labelWidth;
        layoutElement.flexibleWidth = 0f;

        string labelText = (displayName + ":").ToUpper();
        labelText = labelText.Replace(" ", new string(' ', 8));

        textComponent.text = labelText;
        textComponent.font = statFont;
        textComponent.fontSize = mainTextFontSize;
        textComponent.alignment = TextAlignmentOptions.Left;
        textComponent.color = Color.black;
        textComponent.textWrappingMode = TextWrappingModes.NoWrap;
        textComponent.overflowMode = TextOverflowModes.Overflow;
    }

    private void ConfigureMainText(TextMeshProUGUI textComponent)
    {
        float mainValueWidth = statElementWidth * mainValueWidthRatio;

        LayoutElement layoutElement = textComponent.gameObject.AddComponent<LayoutElement>();
        layoutElement.minWidth = mainValueWidth;
        layoutElement.preferredWidth = mainValueWidth;
        layoutElement.flexibleWidth = 0f;

        textComponent.text = "0";
        textComponent.font = statFont;
        textComponent.fontSize = mainTextFontSize;
        textComponent.alignment = TextAlignmentOptions.Left;
        textComponent.color = Color.black;
        textComponent.textWrappingMode = TextWrappingModes.NoWrap;
        textComponent.overflowMode = TextOverflowModes.Overflow;
    }

    private void ConfigureAdditiveText(TextMeshProUGUI textComponent)
    {
        float additiveValueWidth = statElementWidth * additiveValueWidthRatio;

        LayoutElement layoutElement = textComponent.gameObject.AddComponent<LayoutElement>();
        layoutElement.minWidth = additiveValueWidth;
        layoutElement.preferredWidth = additiveValueWidth;
        layoutElement.flexibleWidth = 0f;

        textComponent.text = "";
        textComponent.font = statFont;
        textComponent.fontSize = additiveTextFontSize;
        textComponent.alignment = TextAlignmentOptions.Left;
        textComponent.color = Color.gray;
        textComponent.textWrappingMode = TextWrappingModes.NoWrap;
        textComponent.overflowMode = TextOverflowModes.Overflow;
    }

    private string GetStatDisplayName(StatType statType)
    {
        FieldInfo field = typeof(StatType).GetField(statType.ToString());
        StatDisplayAttribute attribute = field?.GetCustomAttribute<StatDisplayAttribute>();
        return attribute?.DisplayName ?? statType.ToString();
    }

    private void UpdateStat(StatType statType, float currentValue, float defaultValue)
    {
        if (!statElements.ContainsKey(statType)) return;

        float additive = currentValue - defaultValue;
        StatUIElement element = statElements[statType];

        element.MainText.text = currentValue.ToString("F2").ToLower();

        if (additive > 0)
        {
            element.AdditiveText.text = $"(+{additive:F2})".ToLower();
            element.AdditiveText.color = Color.green;
        }
        else if (additive < 0)
        {
            element.AdditiveText.text = $"({additive:F2})".ToLower();
            element.AdditiveText.color = Color.red;
        }
        else
        {
            element.AdditiveText.text = "";
        }
    }
}
