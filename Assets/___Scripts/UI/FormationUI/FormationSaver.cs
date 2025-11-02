using UnityEngine;
using System.Collections.Generic;

public class FormationSaver : MonoBehaviour
{
    public RectTransform formationPanel;     // assign in Inspector
    public RectTransform formationCenter;    // assign in Inspector

    [System.Serializable]
    public class UnitPosition
    {
        public string name;
        public Vector2 offset;
    }

    public List<UnitPosition> unitPositions = new();

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
            SaveFormation();
    }

    public void SaveFormation()
    {
        unitPositions.Clear();

        foreach (RectTransform child in formationPanel)
        {
            // Skip the center object
            if (child == formationCenter)
                continue;

            // Compute offset relative to the center
            Vector2 offset = child.anchoredPosition - formationCenter.anchoredPosition;

            unitPositions.Add(new UnitPosition
            {
                name = child.name,
                offset = offset
            });

            Debug.Log($"Saved unit {child.name} at {offset} offset.");
        }

        Debug.Log($"Saved {unitPositions.Count} unit positions.");
    }
}
