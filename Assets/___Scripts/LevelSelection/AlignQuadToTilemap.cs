using UnityEngine;
using UnityEngine.Tilemaps;

public class AlignQuadToTilemap : MonoBehaviour
{
    public Tilemap tilemap;
    public Transform quadTransform; // the transform of the quad

    [ContextMenu("Align Quad")]
    void Align()
    {
        // Get bounds in world space
        Bounds bounds = tilemap.localBounds;

        // Tilemap localBounds are relative to tilemap transform
        // Convert to world center if tilemap is not at (0,0)
        Vector3 worldCenter = tilemap.transform.TransformPoint(bounds.center);
        Vector3 worldSize = Vector3.Scale(bounds.size, tilemap.transform.lossyScale);

        // Position the quad
        quadTransform.position = worldCenter;

        // Scale the quad (Quad 1x1 = 1 world unit height & width)
        quadTransform.localScale = new Vector3(worldSize.x, worldSize.y, 1f);

        Debug.Log($"Quad aligned to tilemap. Size={worldSize}, Center={worldCenter}");
    }
}
