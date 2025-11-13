using UnityEngine;

[RequireComponent(typeof(PolygonCollider2D))]
[RequireComponent(typeof(LineRenderer))]
public class PolygonOutline : MonoBehaviour
{
    private void Awake()
    {
        PolygonCollider2D poly = GetComponent<PolygonCollider2D>();
        LineRenderer lr = GetComponent<LineRenderer>();

        // Smooth corners visually
        lr.numCornerVertices = 5;
        lr.numCapVertices = 5;

        // Recommended for map outlines
        lr.useWorldSpace = false;
        lr.loop = true;


        // Generate the outline based on *all* collider paths
        CreateOutline(poly, lr);
    }

    private void CreateOutline(PolygonCollider2D poly, LineRenderer lr)
    {
        int totalPoints = 0;

        // PolygonCollider2D can have multiple paths (like holes or separated regions)
        for (int i = 0; i < poly.pathCount; i++)
            totalPoints += poly.GetPath(i).Length;

        // +1 only if you want a closed loop for the whole shape (not per-path)
        lr.positionCount = totalPoints;

        int index = 0;

        // Copy collider paths into the LineRenderer
        for (int p = 0; p < poly.pathCount; p++)
        {
            Vector2[] path = poly.GetPath(p);

            for (int i = 0; i < path.Length; i++)
            {
                lr.SetPosition(index, path[i]);
                index++;
            }
        }
    }
}
