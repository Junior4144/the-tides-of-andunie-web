using System.Linq;
using UnityEngine;

public class DebugFormationSaver : MonoBehaviour
{
    void Start()
    {
        TrySaveFormation();
    }

    private void TrySaveFormation()
    {
        if (!HasValidManager()) return;

        ClearAndSaveFormation();
        Destroy(gameObject);
    }

    private bool HasValidManager()
    {
        if (SquadFormationManager.Instance != null) return true;

        Debug.LogError("[DebugFormationSaver] FormationSquadManager.Instance is null. Cannot save formation.");
        return false;
    }

    private void ClearAndSaveFormation()
    {
        SquadFormationManager.Instance.ClearFormation();
        SaveChildrenToFormation();
        LogFormationSaved();
    }

    private void SaveChildrenToFormation()
    {
        Vector2 parentPos = transform.position;

        transform.Cast<Transform>()
            .ToList()
            .ForEach(child => AddChildToFormation(child, parentPos));
    }

    private void AddChildToFormation(Transform child, Vector2 parentPos)
    {
        UnitIdentifier unitId = child.GetComponent<UnitIdentifier>();

        if (!ValidateUnit(unitId, child.name)) return;

        Vector2 offset = (Vector2)child.position - parentPos;
        UnitType unitType = unitId.UnitType;

        Debug.Log($"[DebugFormationSaver] Unit saved {unitType}");
        Debug.Log($"[DebugFormationSaver] Position {child.position}");
        Debug.Log($"[DebugFormationSaver] Offset {offset}");

        SquadFormationManager.Instance.AddUnitToFormation(unitType, offset);
    }

    private bool ValidateUnit(UnitIdentifier unitId, string childName)
    {
        if (unitId == null)
        {
            Debug.LogWarning($"[DebugFormationSaver] Skipping {childName} - missing UnitIdentifier");
            return false;
        }

        return true;
    }

    private void LogFormationSaved()
    {
        Debug.Log($"[DebugFormationSaver] Total units {transform.childCount}");
    }
}
