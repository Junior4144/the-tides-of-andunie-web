using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;

public class RemoveHiddenTiles : EditorWindow
{
    public Tilemap topTilemap;     // Trees
    public Tilemap bottomTilemap;  // Ground

    [MenuItem("Tools/Tilemap/Remove Hidden Tiles")]
    public static void ShowWindow()
    {
        GetWindow(typeof(RemoveHiddenTiles));
    }

    private void OnGUI()
    {
        GUILayout.Label("Remove interior bottom tiles fully surrounded by top tiles", EditorStyles.boldLabel);

        topTilemap = (Tilemap)EditorGUILayout.ObjectField("Top Tilemap (trees)", topTilemap, typeof(Tilemap), true);
        bottomTilemap = (Tilemap)EditorGUILayout.ObjectField("Bottom Tilemap (ground)", bottomTilemap, typeof(Tilemap), true);

        if (GUILayout.Button("Run Cleanup (Edge + Corner Safe)"))
        {
            if (topTilemap == null || bottomTilemap == null)
            {
                Debug.LogError("[TilemapCleanup] Assign both tilemaps first!");
                return;
            }

            RemoveHidden();
        }
    }

    private void RemoveHidden()
    {
        int removedCount = 0;

        foreach (Vector3Int pos in topTilemap.cellBounds.allPositionsWithin)
        {
            if (!HasTile(topTilemap, pos))
                continue;

            // Check 8 neighbors (including diagonals)
            if (HasTile(topTilemap, pos + new Vector3Int(1, 0, 0)) &&  // Right
                HasTile(topTilemap, pos + new Vector3Int(-1, 0, 0)) &&  // Left
                HasTile(topTilemap, pos + new Vector3Int(0, 1, 0)) &&  // Up
                HasTile(topTilemap, pos + new Vector3Int(0, -1, 0)) &&  // Down

                HasTile(topTilemap, pos + new Vector3Int(1, 1, 0)) &&  // Up-right
                HasTile(topTilemap, pos + new Vector3Int(-1, 1, 0)) &&  // Up-left
                HasTile(topTilemap, pos + new Vector3Int(1, -1, 0)) &&  // Down-right
                HasTile(topTilemap, pos + new Vector3Int(-1, -1, 0)))    // Down-left
            {
                // Only delete ground if ALL 8 neighbors are trees
                if (HasTile(bottomTilemap, pos))
                {
                    bottomTilemap.SetTile(pos, null);
                    removedCount++;
                }
            }
        }

        Debug.Log($"[TilemapCleanup] Removed {removedCount} ground tiles (edge + corner safe).");
        EditorUtility.SetDirty(bottomTilemap);
    }

    private bool HasTile(Tilemap map, Vector3Int pos)
    {
        return map != null && map.GetTile(pos) != null;
    }
}