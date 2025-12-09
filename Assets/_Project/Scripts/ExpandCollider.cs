using DG.Tweening;
using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(Transform))]
public class ExpandCollider : MonoBehaviour
{
    [Header("Tween Settings")]
    [SerializeField] Vector3 startSize = Vector3.zero;
    [SerializeField] Vector3 endSize = Vector3.one;
    [SerializeField] float duration = 0.35f;


    void OnEnable()
    {
        var t = transform;
        t.localScale = startSize;
        t.DOScale(endSize, duration).SetEase(Ease.Linear);
    }

    void OnDrawGizmos()
    {
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.color = Color.cyan;

        Collider2D col = GetComponent<Collider2D>();
        if (!col) return;

        switch (col)
        {
            case CircleCollider2D circle:
                Gizmos.DrawWireSphere(circle.offset, circle.radius);
                break;

            case BoxCollider2D box:
                Gizmos.DrawWireCube(box.offset, box.size);
                break;

            case CapsuleCollider2D capsule:
                Gizmos.DrawWireCube(capsule.offset, capsule.size);
                break;

            case PolygonCollider2D poly:
                DrawPolygon(poly);
                break;

            default:
                Gizmos.DrawWireCube(Vector3.zero, endSize);
                break;
        }
    }

    void DrawPolygon(PolygonCollider2D poly)
    {
        var points = poly.points;
        if (points.Length < 2) return;

        for (int i = 0; i < points.Length; i++)
        {
            Vector2 current = points[i] + poly.offset;
            Vector2 next = points[(i + 1) % points.Length] + poly.offset;
            Gizmos.DrawLine(current, next);
        }
    }
}
