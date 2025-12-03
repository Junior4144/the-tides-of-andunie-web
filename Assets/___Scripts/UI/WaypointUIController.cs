using UnityEngine;

public class WaypointUIController : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private float _edgePadding = 50f;

    void Update()
    {
        if (GlobalStoryManager.Instance == null) return;

        if (!GlobalStoryManager.Instance.showWaypoints) // if is false
        {
            gameObject.SetActive(false);
            return;
        }


        if (_target == null || Camera.main == null)
            return;

        Vector3 targetScreenPos = Camera.main.WorldToScreenPoint(_target.position);
        Vector3 clampedScreenPos = ClampToScreenEdges(targetScreenPos);

        transform.SetPositionAndRotation(
            clampedScreenPos,
            CalculateRotationTowards(targetScreenPos, clampedScreenPos)
        );
    }

    private Vector3 ClampToScreenEdges(Vector3 screenPos)
    {
        return new Vector3(
            Mathf.Clamp(screenPos.x, _edgePadding, Screen.width - _edgePadding),
            Mathf.Clamp(screenPos.y, _edgePadding, Screen.height - _edgePadding),
            screenPos.z
        );
    }

    private Quaternion CalculateRotationTowards(Vector3 targetPos, Vector3 waypointPos)
    {
        Vector2 direction = (waypointPos - targetPos).normalized;
        float angle = Mathf.Atan2(-direction.x, direction.y) * Mathf.Rad2Deg;
        return Quaternion.Euler(0, 0, angle);
    }
}
