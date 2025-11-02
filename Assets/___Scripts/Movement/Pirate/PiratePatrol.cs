using UnityEngine;
using UnityEngine.AI;

public class PiratePatrol : MonoBehaviour
{
    [SerializeField] private Transform[] patrolPoints;
    [SerializeField] private float waitTime = 2f;
    [SerializeField] private float awarenessDistance = 10f;
    [SerializeField] private PirateAttributes _attributes;

    private Transform player;
    private int currentPointIndex = 0;
    private NavMeshAgent agent;
    private float waitTimer;
    private Transform currentPatrolPoint;
    private Rigidbody2D _rigidbody;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        _rigidbody = GetComponent<Rigidbody2D>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        if (HasPatrolPoints())
        {
            agent.SetDestination(patrolPoints[0].position);
            currentPatrolPoint = patrolPoints[0];
        }
    }

    private bool HasPatrolPoints() => patrolPoints != null && patrolPoints.Length > 0;

    void Update()
    {
        if (!agent.enabled || !PlayerManager.Instance) return;

        player = PlayerManager.Instance.transform;
        RotateTowardsMovementDirection();

        if (!HasPatrolPoints())
        {
            agent.SetDestination(player.position);
            return;
        }

        if (Vector2.Distance(transform.position, player.position) <= awarenessDistance)
        {
            currentPatrolPoint = player;
            agent.SetDestination(player.position);
            return;
        }

        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            waitTimer += Time.deltaTime;

            if (waitTimer >= waitTime)
            {
                currentPointIndex = (currentPointIndex + 1) % patrolPoints.Length;
                currentPatrolPoint = patrolPoints[currentPointIndex];
                agent.SetDestination(currentPatrolPoint.position);
                waitTimer = 0f;
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        foreach (Transform point in patrolPoints)
            Gizmos.DrawSphere(point.position, 0.2f);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, awarenessDistance);
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
