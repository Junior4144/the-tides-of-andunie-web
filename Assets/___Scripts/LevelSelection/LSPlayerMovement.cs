using UnityEngine;
using UnityEngine.AI;

public class LSPlayerMovement : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 360f;
    public Vector2 cursorHotSpot = Vector2.zero;

    NavMeshAgent agent;
    Camera cam;
    Vector3 smoothDir;

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

        RotateTowardVelocity();
    }

    void TryMoveToMouse()
    {
        Vector2 mouseWorld = cam.ScreenToWorldPoint(Input.mousePosition);
        bool validNavMesh = NavMesh.SamplePosition(mouseWorld, out NavMeshHit hit, 0.5f, NavMesh.AllAreas);

        bool validCollider = false;

        Collider2D[] hits = Physics2D.OverlapPointAll(mouseWorld);
        Collider2D currentColldier = null;
        foreach (var h in hits)
        {
            if (h.CompareTag("LSVillagePointerTarget"))
            {
                validCollider = true;
                currentColldier = h;
                Debug.Log("Hit the correct Village Pointer Target!");
                break;
            }
        }

        if (validNavMesh)
        {
            agent.SetDestination(hit.position);
        }
        else if (validCollider)
        {
            var curr = currentColldier.GetComponent<VillagePointerTargetController>().navigationTarget;
            agent.SetDestination(curr.transform.position);
        }
        else
        {
            Debug.Log("Cannot move there — no NavMesh.");
        }
    }

    void RotateTowardVelocity()
    {
        Vector3 vel = agent.velocity;
        if (vel.sqrMagnitude > 0.01f)
        {
            float angle = Mathf.Atan2(vel.y, vel.x) * Mathf.Rad2Deg - 90f;
            Quaternion targetRot = Quaternion.AngleAxis(angle, Vector3.forward);

            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                targetRot,
                rotationSpeed * Time.deltaTime
            );
        }
    }
}
