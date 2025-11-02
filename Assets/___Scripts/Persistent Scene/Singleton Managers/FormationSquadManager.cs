using System.Collections.Generic;
using UnityEngine;

public struct FormationPosition
{
    public GameObject unitPrefab;
    public Vector2 offset;

    public FormationPosition(GameObject unitPrefab, Vector2 offset)
    {
        this.unitPrefab = unitPrefab;
        this.offset = offset;
    }
}

public class FormationSquadManager : MonoBehaviour
{
    public static FormationSquadManager Instance { get; private set; }

    [SerializeField]
    private List<FormationPosition> _formationPositions = new();

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void AddUnitToFormation(GameObject unitPrefab, Vector2 offset) =>
        _formationPositions.Add(new(unitPrefab, offset));

    public IReadOnlyList<FormationPosition> GetFormation() => _formationPositions;

    public void ClearFormation() => _formationPositions.Clear();
}
