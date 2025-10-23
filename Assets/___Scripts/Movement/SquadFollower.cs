using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(NavMeshAgent))]
public class SquadFollower : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float maxMoveSpeed = 20f;
    [SerializeField] private float stoppingDistance = 0.01f;
    [SerializeField] private float rotationSpeed = 360f;
    [SerializeField] private float navMeshEngageDistance = 4f;
    

    private Transform squad;
    private Rigidbody2D rb;
    private NavMeshAgent navAgent;

    private Vector3 formationOffsetLocal;
    private float formationAngleLocal;
    private Vector3 targetPositionInFormation;
    private SquadImpulseController _squadImpulseController;
    private bool usingNavMesh = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        navAgent = GetComponent<NavMeshAgent>();
        _squadImpulseController = GetComponentInParent<SquadImpulseController>();

        navAgent.speed = maxMoveSpeed;
        navAgent.angularSpeed = rotationSpeed;
        navAgent.radius = 0.25f;
        navAgent.acceleration = 100f;
        navAgent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
        navAgent.updateRotation = false;
        navAgent.updateUpAxis = false;
        navAgent.enabled = false;

        if (transform.parent != null)
        {
            squad = transform.parent;

            Vector3 worldOffset = transform.position - squad.position;
            formationOffsetLocal = Quaternion.Inverse(squad.rotation) * worldOffset;
            formationAngleLocal = transform.eulerAngles.z - squad.eulerAngles.z;
        }
        else
        {
            Debug.LogWarning($"{gameObject.name}: Could not find squad parent object.");
        }

        transform.SetParent(null);
    }

    void Update()
    {
        if (squad == null)
        {
            Destroy(gameObject);
            return;
        }

        if (_squadImpulseController.IsInImpulse()) return;

        float distanceToFormation = Vector3.Distance(transform.position, targetPositionInFormation);

        if (distanceToFormation > navMeshEngageDistance)
        {
            if (!usingNavMesh)
            {
                EnableNavMesh();
            }
            navAgent.SetDestination(targetPositionInFormation);
            RotateTowardsMovementDirection();
        }
        else
        {
            if (usingNavMesh)
            {
                DisableNavMesh();
            }
            MoveTowardsFormationPosition();
        }
    }

    void LateUpdate()
    {
        if (squad == null)
        {
            Debug.LogError($"Squad not found for unit: {rb}");
            return;
        }

        Vector3 unitOffsetFromSquadCenter = squad.rotation * formationOffsetLocal;
        targetPositionInFormation = squad.position + unitOffsetFromSquadCenter;
    }

    private void EnableNavMesh()
    {
        usingNavMesh = true;
        navAgent.enabled = true;
    }

    private void DisableNavMesh()
    {
        usingNavMesh = false;
        navAgent.enabled = false;
    }

    private void MoveTowardsFormationPosition()
    {
        Vector3 directionToTarget = targetPositionInFormation - transform.position;
        float distanceToTarget = directionToTarget.magnitude;

        MatchFormationAngle();

        if (distanceToTarget > stoppingDistance)
            SetVelocity(directionToTarget.normalized * Mathf.Min(moveSpeed * distanceToTarget, maxMoveSpeed));
        else
            SetVelocity(Vector2.zero);
    }

    private void RotateTowardsMovementDirection()
    {
        Vector3 velocity = navAgent.velocity;
        
        if (velocity.sqrMagnitude > 0.01f)
        {
            float targetAngle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg - 90;
            SetRotation(targetAngle);
        }
    }

    private void MatchFormationAngle() =>
        SetRotation(targetAngle: squad.eulerAngles.z + formationAngleLocal);

    private void SetRotation(float targetAngle)
    {
        float currentAngle = rb.rotation;
        float newAngle = Mathf.MoveTowardsAngle(currentAngle, targetAngle, rotationSpeed * Time.deltaTime);
        rb.SetRotation(newAngle);
    }

    private void SetVelocity(Vector2 velocity)
    {   
        rb.linearVelocity = velocity;
    }
    
}