using System;
using System.Collections.Generic;
using UnityEngine;

public enum UnitType
{
    PlayerMelee,
    PirateMelee,
    PirateHero
}

[Serializable]
public struct UnitPrefabMapping
{
    public UnitType unitType;
    public GameObject prefab;
}

public class UnitRegistry : MonoBehaviour
{
    [SerializeField] private List<UnitPrefabMapping> _unitMappings = new();

    private Dictionary<UnitType, GameObject> _prefabLookup;

    void Awake()
    {
        BuildPrefabLookup();
    }

    private void BuildPrefabLookup()
    {
        _prefabLookup = new Dictionary<UnitType, GameObject>();

        foreach (var mapping in _unitMappings)
        {
            _prefabLookup[mapping.unitType] = mapping.prefab;
        }
    }

    public GameObject GetPrefab(UnitType unitType)
    {
        if (_prefabLookup.TryGetValue(unitType, out GameObject prefab))
            return prefab;

        Debug.LogError($"[UnitRegistry] No prefab found for {unitType}");
        return null;
    }
}
