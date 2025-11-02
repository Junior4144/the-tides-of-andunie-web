using System.Linq;
using UnityEngine;

public class DebugFormationSaver : MonoBehaviour
{
    [SerializeField] private bool saveOnStart = true;
    [SerializeField] private KeyCode saveKey = KeyCode.M;

    void Start()
    {
        if (saveOnStart) TrySaveFormation();
    }

    void Update()
    {
        if (Input.GetKeyDown(saveKey)) TrySaveFormation();
    } 

    private void TrySaveFormation()
    {
        if (!HasValidManager()) return;

        ClearAndSaveFormation();
        Destroy(gameObject);
    }

    private bool HasValidManager()
    {
        if (FormationSquadManager.Instance != null) return true;

        Debug.LogError("[DebugFormationSaver] FormationSquadManager.Instance is null. Cannot save formation.");
        return false;
    }

    private void ClearAndSaveFormation()
    {
        FormationSquadManager.Instance.ClearFormation();
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
        Vector2 offset = (Vector2)child.position - parentPos;
        string prefabName = child.gameObject.name;

        Debug.Log($"[DebugFormationSaver] Unit saved {prefabName}");
        Debug.Log($"[DebugFormationSaver] Position {child.position}");
        Debug.Log($"[DebugFormationSaver] Offset {offset}");

        FormationSquadManager.Instance.AddUnitToFormation(child.gameObject, offset);
    }

    private void LogFormationSaved()
    {
        Debug.Log($"[DebugFormationSaver] Total units {transform.childCount}");
    }
}
