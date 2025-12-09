using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(PolygonCollider2D))]
[RequireComponent(typeof(LineRenderer))]
public class PolygonOutline : MonoBehaviour
{
    public Color fillColor = new Color(1f, 1f, 1f, 0.25f);
    private RegionInfo regionInfo;
    private void Awake()
    {
        regionInfo = GetComponent<RegionInfo>();
    }
    private void OnEnable() => SceneManager.activeSceneChanged += HandleCheck;

    private void OnDisable() => SceneManager.activeSceneChanged -= HandleCheck;

    private void HandleCheck(Scene oldScene, Scene newScene)
    {
        StartCoroutine(CheckAfterLoading(newScene));
    }

    private IEnumerator CheckAfterLoading(Scene newScene)
    {
        yield return null;

        if (newScene == gameObject.scene)
            HandlingPolygonSetup();
    }

    private void HandlingPolygonSetup()
    {
        PolygonCollider2D poly = GetComponent<PolygonCollider2D>();
        LineRenderer lr = GetComponent<LineRenderer>();
        
        lr.numCornerVertices = 5;
        lr.numCapVertices = 5;
        
        lr.useWorldSpace = false;
        lr.loop = true;


        if (!LSRegionLockManager.Instance.IsRegionLocked(regionInfo))
        {
            fillColor.a = 0f;
        }
        
        CreateOutline(poly, lr);
        CreateFill(poly);
    }

    private void CreateOutline(PolygonCollider2D poly, LineRenderer lr)
    {
        int totalPoints = 0;
        
        for (int i = 0; i < poly.pathCount; i++)
            totalPoints += poly.GetPath(i).Length;
        
        lr.positionCount = totalPoints;

        int index = 0;
        
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

    private void CreateFill(PolygonCollider2D poly)
    {
        MeshFilter mf = GetComponent<MeshFilter>();
        MeshRenderer mr = GetComponent<MeshRenderer>();

        mr.sortingLayerName = "UI";
        mr.sortingOrder = 10;

        // Collect all polygon points into one array
        List<Vector2> points2D = new List<Vector2>();

        for (int p = 0; p < poly.pathCount; p++)
        {
            Vector2[] path = poly.GetPath(p);
            points2D.AddRange(path);
        }

        // Convert to 3D vertices
        Vector3[] vertices = new Vector3[points2D.Count];
        for (int i = 0; i < points2D.Count; i++)
            vertices[i] = points2D[i];

        // Triangulate
        Triangulator triangulator = new Triangulator(points2D.ToArray());
        int[] triangles = triangulator.Triangulate();

        // Build mesh
        Mesh mesh = new Mesh
        {
            vertices = vertices,
            triangles = triangles
        };

        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        mf.mesh = mesh;

        // Apply transparent material
        Material mat = new Material(Shader.Find("Sprites/Default"));
        mat.color = fillColor;
        mr.material = mat;
    }
    
    public class Triangulator
    {
        private List<Vector2> points;

        public Triangulator(Vector2[] points)
        {
            this.points = new List<Vector2>(points);
        }

        public int[] Triangulate()
        {
            List<int> indices = new List<int>();

            int n = points.Count;
            if (n < 3)
                return indices.ToArray();

            int[] V = new int[n];
            if (Area() > 0)
                for (int v = 0; v < n; v++) V[v] = v;
            else
                for (int v = 0; v < n; v++) V[v] = (n - 1) - v;

            int nv = n;
            int count = 2 * nv;

            for (int v = nv - 1; nv > 2;)
            {
                if ((count--) <= 0)
                    return indices.ToArray();

                int u = v < nv ? v : 0;
                v = (u + 1) % nv;
                int w = (v + 1) % nv;

                if (Snip(u, v, w, nv, V))
                {
                    indices.Add(V[u]);
                    indices.Add(V[v]);
                    indices.Add(V[w]);

                    for (int s = v; s < nv - 1; s++)
                        V[s] = V[s + 1];

                    nv--;
                    count = 2 * nv;
                }
            }

            return indices.ToArray();
        }

        float Area()
        {
            float A = 0f;
            int n = points.Count;

            for (int p = n - 1, q = 0; q < n; p = q++)
                A += points[p].x * points[q].y - points[q].x * points[p].y;

            return A * 0.5f;
        }

        bool Snip(int u, int v, int w, int n, int[] V)
        {
            Vector2 A = points[V[u]];
            Vector2 B = points[V[v]];
            Vector2 C = points[V[w]];

            if (Mathf.Epsilon > (((B.x - A.x) * (C.y - A.y)) -
                                 ((B.y - A.y) * (C.x - A.x))))
                return false;

            for (int p = 0; p < n; p++)
            {
                if (p == u || p == v || p == w) continue;
                Vector2 P = points[V[p]];
                if (InsideTriangle(A, B, C, P))
                    return false;
            }

            return true;
        }

        bool InsideTriangle(Vector2 A, Vector2 B, Vector2 C, Vector2 P)
        {
            float ax = C.x - B.x, ay = C.y - B.y;
            float bx = A.x - C.x, by = A.y - C.y;
            float cx = B.x - A.x, cy = B.y - A.y;

            float apx = P.x - A.x, apy = P.y - A.y;
            float bpx = P.x - B.x, bpy = P.y - B.y;
            float cpx = P.x - C.x, cpy = P.y - C.y;

            float aCROSSbp = ax * bpy - ay * bpx;
            float cCROSSap = cx * apy - cy * apx;
            float bCROSScp = bx * cpy - by * cpx;

            return (aCROSSbp >= 0f) && (bCROSScp >= 0f) && (cCROSSap >= 0f);
        }
    }
}
