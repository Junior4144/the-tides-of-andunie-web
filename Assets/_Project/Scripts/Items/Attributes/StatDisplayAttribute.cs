using System;

[AttributeUsage(AttributeTargets.Field)]
public class StatDisplayAttribute : Attribute
{
    public string DisplayName { get; }

    public StatDisplayAttribute(string displayName)
    {
        DisplayName = displayName;
    }
}
