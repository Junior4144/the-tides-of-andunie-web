using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "VillageBuildingData", menuName = "Village Invasion/Building Data")]
public class VillageBuildingData : ScriptableObject
{
    [Header("Fire Settings")]
    public GameObject fireSoundPrefab;
    public GameObject fireSpritePrefab;

    [Header("Destroyed Sprites")]
    public List<Sprite> destroyedSprites;

    [Serializable]
    public struct Rule
    {
        public string keyword;
        public Sprite destroyedSprite;
    }

    [Header("List of name-based destroyed sprite rules")]
    public List<Rule> rules;

    public Sprite GetDestroyedSprite(string buildingName)
    {
        string lowerName = buildingName.ToLower();

        foreach (var rule in rules)
        {
            if (lowerName.Contains(rule.keyword.ToLower()))
                return rule.destroyedSprite;
        }

        Debug.LogWarning($"No matching rule for building name: {buildingName}");
        return null;
    }
}

