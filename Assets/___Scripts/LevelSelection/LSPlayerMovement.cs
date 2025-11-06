using UnityEngine;
using UnityEngine.AI;

public class LSPlayerMovement : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 360f;
    public Vector2 cursorHotSpot = Vector2.zero;

    NavMeshAgent agent;
    Camera cam;
    Vector3 smoothDir;

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
        if (Input.GetMouseButtonDown(1))
            TryMoveToMouse();

        RotateTowardVelocity();
    }

    void TryMoveToMouse()
    {
        Vector2 mouseWorld = cam.ScreenToWorldPoint(Input.mousePosition);
        if (NavMesh.SamplePosition(mouseWorld, out NavMeshHit hit, 0.5f, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
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
