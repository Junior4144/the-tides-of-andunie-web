using UnityEngine;
public class Triangulator
{
    private System.Collections.Generic.List<Vector2> m_points;

    public Triangulator(Vector2[] points)
    {
        m_points = new System.Collections.Generic.List<Vector2>(points);
    }

    public int[] Triangulate()
    {
        var indices = new System.Collections.Generic.List<int>();

        int n = m_points.Count;
        if (n < 3)
            return indices.ToArray();

        int[] V = new int[n];
        if (Area() > 0)
            for (int v = 0; v < n; v++) V[v] = v;
        else
            for (int v = 0; v < n; v++) V[v] = (n - 1) - v;

        int nv = n;
        int count = 2 * nv;
        for (int m = 0, v = nv - 1; nv > 2;)
        {
            if ((count--) <= 0) return indices.ToArray();

            int u = v; if (nv <= u) u = 0;
            v = u + 1; if (nv <= v) v = 0;
            int w = v + 1; if (nv <= w) w = 0;

            if (Snip(u, v, w, nv, V))
            {
                int a = V[u];
                int b = V[v];
                int c = V[w];

                indices.Add(a);
                indices.Add(b);
                indices.Add(c);

                for (int s = v, t = v + 1; t < nv; s++, t++)
                    V[s] = V[t];
                nv--;
                count = 2 * nv;
            }
        }

        return indices.ToArray();
    }

    float Area()
    {
        int n = m_points.Count;
        float A = 0f;

        for (int p = n - 1, q = 0; q < n; p = q++)
            A += m_points[p].x * m_points[q].y - m_points[q].x * m_points[p].y;

        return A * 0.5f;
    }

    bool Snip(int u, int v, int w, int n, int[] V)
    {
        Vector2 A = m_points[V[u]];
        Vector2 B = m_points[V[v]];
        Vector2 C = m_points[V[w]];

        if (Mathf.Epsilon > (((B.x - A.x) * (C.y - A.y)) -
                             ((B.y - A.y) * (C.x - A.x))))
            return false;

        for (int p = 0; p < n; p++)
        {
            if ((p == u) || (p == v) || (p == w)) continue;
            Vector2 P = m_points[V[p]];

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

        return aCROSSbp >= 0 && bCROSScp >= 0 && cCROSSap >= 0;
    }
}

[RequireComponent(typeof(LineRenderer), typeof(MeshFilter), typeof(MeshRenderer))]
public class LineRendererFill : MonoBehaviour
{
    public Color fillColor = new Color(1, 1, 1, 0.2f); // transparent white

    void Start()
    {
        LineRenderer lr = GetComponent<LineRenderer>();
        MeshFilter mf = GetComponent<MeshFilter>();
        MeshRenderer mr = GetComponent<MeshRenderer>();

        int count = lr.positionCount;
        Vector3[] positions = new Vector3[count];
        lr.GetPositions(positions);

        // Convert to 2D
        Vector2[] polyPoints = new Vector2[count];
        for (int i = 0; i < count; i++)
            polyPoints[i] = new Vector2(positions[i].x, positions[i].y);

        // Triangulate the polygon
        Triangulator triangulator = new Triangulator(polyPoints);
        int[] triangles = triangulator.Triangulate();

        // Create mesh
        Mesh mesh = new Mesh();
        Vector3[] verts3D = new Vector3[count];
        for (int i = 0; i < count; i++)
            verts3D[i] = positions[i];

        mesh.vertices = verts3D;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        mf.mesh = mesh;

    }
}
