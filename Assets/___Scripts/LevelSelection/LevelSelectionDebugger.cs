using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelSelectionDebugger : MonoBehaviour
{
    [Header("LS Debugger Settings")]
    public bool countTilesOnStart = true;

    void Start()
    {
        if (countTilesOnStart)
            CountAllTiles();
    }

    public void CountAllTiles()
    {
        Tilemap[] maps = FindObjectsByType<Tilemap>(FindObjectsSortMode.None);

        int total = 0;
        foreach (Tilemap tm in maps)
        {
            int mapCount = 0;
            foreach (var pos in tm.cellBounds.allPositionsWithin)
                if (tm.HasTile(pos))
                    mapCount++;

            total += mapCount;
            Debug.Log($"[LSdebugger] Tilemap \"{tm.name}\" has {mapCount} tiles.");
        }

        Debug.Log($"[LSdebugger] TOTAL tiles in scene: {total}");
    }
}
