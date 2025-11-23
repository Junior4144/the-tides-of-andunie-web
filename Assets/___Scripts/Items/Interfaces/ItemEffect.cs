using UnityEngine;
using System;
using System.Reflection;

public class ItemEffect : MonoBehaviour
{
    [SerializeField] private StatType statType;
    [SerializeField] private float amount;
    [SerializeField] private bool _isPercentage;
    internal readonly int Amount;
    [HideInInspector] public float ItemAmount => amount;
    public bool IsPercentage => _isPercentage;

    public void Apply()
    {
        var currentValue = GetCurrentStatValue();
        var newValue = CalculateNewValue(currentValue);

        ApplyStatChange(newValue);
    }

    private float GetCurrentStatValue()
    {
        var property = GetStatProperty();

        if (property == null)
        {
            Debug.LogError($"Property '{statType}' not found on PlayerStatsManager. Please add the property.");
            return 0f;
        }

        return (float)property.GetValue(PlayerStatsManager.Instance);
    }

    private float CalculateNewValue(float currentValue) =>
        IsPercentage
            ? currentValue * (1 + amount / 100f)
            : currentValue + amount;

    private void ApplyStatChange(float newValue)
    {
        var setter = GetSetterMethod();

        if (setter == null)
        {
            Debug.LogError($"Setter method 'Set{statType}' not found on PlayerStatsManager. Please add the method.");
            return;
        }

        setter.Invoke(PlayerStatsManager.Instance, new object[] { newValue });
    }

    private PropertyInfo GetStatProperty() =>
        typeof(PlayerStatsManager).GetProperty(statType.ToString());

    private MethodInfo GetSetterMethod() =>
        typeof(PlayerStatsManager).GetMethod($"Set{statType}");

    private string GetStatDisplayName()
    {
        var field = typeof(StatType).GetField(statType.ToString());
        var attribute = field.GetCustomAttribute<StatDisplayAttribute>();

        if (attribute == null)
        {
            Debug.LogError($"StatDisplayAttribute missing on {statType}. Please add [StatDisplay(\"Name\")] to the enum value.");
            return "";
        }

        return attribute.DisplayName;
    }

    private string FormatEffectDescription(string statName)
    {
        var sign = amount >= 0 ? "+" : "";
        var suffix = IsPercentage ? "%" : "";

        return $"{sign}{amount}{suffix} {statName}";
    }

    public override string ToString() =>
        FormatEffectDescription(GetStatDisplayName());
}