using NUnit.Framework.Internal;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class LSPlayerMovement : MonoBehaviour
{
    public Vector2 cursorHotSpot = Vector2.zero;
    public GameObject targetPrefab;
    public GameObject pingSoundPrefab;

    NavMeshAgent agent;
    Camera cam;

    [HideInInspector]
    public bool disableClicking = false;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();

        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    private void Start() =>
        cam = CameraManager.Instance.GetCamera();


    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !disableClicking)
            TryMoveToMouse();
    }

    void TryMoveToMouse()
    {
        Vector2 mouseWorld = cam.ScreenToWorldPoint(Input.mousePosition);

        bool validNavMesh = NavMesh.SamplePosition(mouseWorld, out NavMeshHit hit, 0.5f, NavMesh.AllAreas);

        Collider2D[] hits = Physics2D.OverlapPointAll(mouseWorld);

        Collider2D pointerCollider = null;

        foreach (var h in hits)
        {
            if (h.TryGetComponent<RegionInfo>(out var region))
            {
                if (LSRegionLockManager.Instance.IsRegionLocked(region))
                {
                    Debug.Log("Cannot move there — REGION LOCKED.");
                    return;
                }
            }
        }

        foreach (var h in hits)
        {
            if (h.CompareTag("LSVillagePointerTarget"))
            {
                pointerCollider = h;
                break;
            }
        }

        if (validNavMesh)
        {
            SpawnNewTarget(hit.position);
            agent.SetDestination(hit.position);
            return;
        }

        if (pointerCollider != null)
        {
            var controller = pointerCollider.GetComponent<VillagePointerTargetController>();
            Vector3 dest = controller.navigationTarget.transform.position;

            SpawnNewTarget(dest); 
            agent.SetDestination(dest);
            return;
        }

        Debug.Log("Cannot move there — no NavMesh.");
    }

    public void SpawnNewTarget(Vector3 position)
    {
        TargetEvents.OnClearAllTargets?.Invoke();
        Instantiate(pingSoundPrefab, position, Quaternion.identity);
        Instantiate(targetPrefab, position, Quaternion.identity);
    }
}
