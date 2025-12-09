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

        agent.radius = .5f;
        agent.obstacleAvoidanceType = ObstacleAvoidanceType.HighQualityObstacleAvoidance;
        agent.avoidancePriority = Random.Range(30, 70);


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
            currentPointIndex = Random.Range(0, patrolPoints.Length);
            currentPatrolPoint = patrolPoints[currentPointIndex];
            agent.SetDestination(currentPatrolPoint.position);
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

            if (waitTimer >= waitTime)
            {
                currentPointIndex = (currentPointIndex + 1) % patrolPoints.Length;
                currentPatrolPoint = patrolPoints[currentPointIndex];
                agent.SetDestination(patrolPoints[currentPointIndex].position);
                waitTimer = 0f;
            }
        }
    }
    private void RotateTowardsMovementDirection()
    {
        Vector3 velocity = agent.velocity;

        // If agent is stationary, don't rotate
        if (velocity.sqrMagnitude < 0.01f) return;

        // Calculate facing angle from movement direction
        float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;

        Quaternion targetRotation = Quaternion.Euler(0, 0, angle - 90f);

        transform.rotation = Quaternion.RotateTowards(
            transform.rotation,
            targetRotation,
            _attributes.RotationSpeed * Time.deltaTime
        );
    }
}
