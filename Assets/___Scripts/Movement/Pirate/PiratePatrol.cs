using UnityEngine;
using UnityEngine.AI;

public class PiratePatrol : MonoBehaviour
{
    public GameObject PatrolPoints;

    private Transform[] patrolPoints;
    public float waitTime = 2f;
    public float awarenessDistance = 10f; // gimzmo to check distance -> you can see as it yellow circle

    public Transform player;
    private int currentPointIndex = 0;
    private NavMeshAgent agent;
    private float waitTimer;
    private Transform currentPatrolPoint;
    private Rigidbody2D _rigidbody;

    [SerializeField] PirateAttributes _attributes;

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

         

        if (patrolPoints.Length > 0)
        {
            agent.SetDestination(patrolPoints[0].position);
            currentPatrolPoint = patrolPoints[0];
        }

    }

    void Update()
    {
        if (!agent.enabled) return;

        if (!PlayerManager.Instance) return;
        
        player = PlayerManager.Instance.transform;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        RotateTowardsTarget(currentPatrolPoint);


        //Player Detection
        if (distanceToPlayer <= awarenessDistance)
        {
            currentPatrolPoint = player.transform;
            RotateTowardsTarget(currentPatrolPoint);
            agent.SetDestination(player.position);
            return;
        }

        //Patrol
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
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

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        foreach (Transform point in patrolPoints)
            Gizmos.DrawSphere(point.position, 0.2f);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, awarenessDistance);
    }
    private void RotateTowardsTarget(Transform currentPatrolPoint)
    {
        if (currentPatrolPoint == null) return;

        Vector2 direction = (currentPatrolPoint.transform.position - transform.position).normalized;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        Quaternion targetRotation = Quaternion.Euler(0, 0, angle - 90f);

        _rigidbody.SetRotation(
            Mathf.MoveTowardsAngle(_rigidbody.rotation, targetRotation.eulerAngles.z, _attributes.RotationSpeed * Time.deltaTime)
        );
    }
}
