using UnityEngine;
using UnityEngine.AI;

public class VillagePatrol : MonoBehaviour
{
    public GameObject PatrolPoints;

    private Transform[] patrolPoints;
    public float waitTime = 2f;

    private int currentPointIndex = 0;
    private NavMeshAgent agent;
    private float waitTimer;
    private Transform currentPatrolPoint;
    private Rigidbody2D _rigidbody;

    [SerializeField] VillagerAttributes _attributes;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        _rigidbody = GetComponent<Rigidbody2D>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        //add prehab position into patrolPoints array
        if (PatrolPoints != null)
        {
            int childCount = PatrolPoints.transform.childCount;
            patrolPoints = new Transform[childCount];

            for (int i = 0; i < childCount; i++)
            {
                patrolPoints[i] = PatrolPoints.transform.GetChild(i);
            }
        }

        if (patrolPoints != null && patrolPoints.Length > 0)
        {
            agent.SetDestination(patrolPoints[0].position);
            currentPatrolPoint = patrolPoints[0];
        }
    }

    void Update()
    {
        if (!agent.enabled) return;

        RotateTowardsMovementDirection();

        if (patrolPoints == null) return;
        

        if (!agent.pathPending && agent.remainingDistance < 1f)
        {
            waitTimer += Time.deltaTime;
            Debug.Log("new destination set attenpt");
            if (waitTimer >= waitTime)
            {
                currentPointIndex = (currentPointIndex + 1) % patrolPoints.Length;
                currentPatrolPoint = patrolPoints[currentPointIndex];
                Debug.Log("new destination set");
                agent.SetDestination(patrolPoints[currentPointIndex].position);
                waitTimer = 0f;
            }
        }
    }
    private void RotateTowardsMovementDirection()
    {
        Vector3 velocity = agent.velocity;

        // If agent is stationary, don’t rotate
        if (velocity.sqrMagnitude < 0.01f) return;

        // Calculate facing angle from movement direction
        float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;

        Quaternion targetRotation = Quaternion.Euler(0, 0, angle - 90f);

        _rigidbody.SetRotation(
            Mathf.MoveTowardsAngle(
                _rigidbody.rotation,
                targetRotation.eulerAngles.z,
                _attributes.RotationSpeed * Time.deltaTime
            )
        );
    }
}
