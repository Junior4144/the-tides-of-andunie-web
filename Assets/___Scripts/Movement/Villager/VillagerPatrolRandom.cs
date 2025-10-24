using UnityEngine;
using UnityEngine.AI;

public class RandomPatrol : MonoBehaviour
{
    public float roamRadius = 50f;
    public float waitTime = 3f;
    public float rotationSpeed = 180f;
    public float minDistance = 20f;
    private NavMeshAgent agent;
    private float waitTimer;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    void Start()
    {
        SetNewRandomDestination();
    }

    void Update()
    {
        RotateTowardsMovementDirection();

        if (!agent.pathPending && agent.remainingDistance <= 1f)
        {
            waitTimer += Time.deltaTime;
            if (waitTimer >= waitTime)
            {
                SetNewRandomDestination();
                waitTimer = 0f;
            }
        }
    }

    void SetNewRandomDestination()
    {
        Vector3 point;
        float distance;

        do
        {
            Vector3 randomDir = Random.insideUnitSphere * roamRadius + transform.position;
            randomDir.z = 0;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomDir, out hit, roamRadius, NavMesh.AllAreas))
            {
                point = hit.position;
                distance = Vector3.Distance(transform.position, point);
            }
            else
            {
                return;
            }
        }
        while (distance < minDistance); // retry until far enough

        agent.SetDestination(point);
    }

    void RotateTowardsMovementDirection()
    {
        Vector3 velocity = agent.velocity;
        if (velocity.sqrMagnitude < 0.01f) return;

        float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
        Quaternion targetRot = Quaternion.Euler(0, 0, angle - 90f);

        transform.rotation = Quaternion.RotateTowards(
            transform.rotation,
            targetRot,
            rotationSpeed * Time.deltaTime
        );
    }
}
