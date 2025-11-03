using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct FormationPosition
{
    public UnitType unitType;
    public Vector2 offset;

    public FormationPosition(UnitType unitType, Vector2 offset)
    {
        this.unitType = unitType;
        this.offset = offset;
    }
}

public class SquadFormationManager : MonoBehaviour
{
    public static SquadFormationManager Instance { get; private set; }

    [SerializeField] private UnitRegistry _unitRegistry;
    [SerializeField] private List<FormationPosition> _formationPositions = new();

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void AddUnitToFormation(UnitType unitType, Vector2 offset) =>
        _formationPositions.Add(new(unitType, offset));

    public GameObject GetPrefab(UnitType unitType) =>
        _unitRegistry.GetPrefab(unitType);

    public IReadOnlyList<FormationPosition> GetFormation() => _formationPositions;

    public void ClearFormation() => _formationPositions.Clear();
}
