using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(NavMeshAgent))]
public class PlayerSquadFollower : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float maxMoveSpeed = 20f;
    [SerializeField] private float stoppingDistance = 0.01f;
    [SerializeField] private float rotationSpeed = 360f;
    [SerializeField] private float navMeshEngageDistance = 4f;
    [SerializeField] private bool debugMode = true;
    

    private Transform player;
    private Rigidbody2D rb;
    private NavMeshAgent navAgent;

    private Vector3 formationOffsetLocal;
    private float formationAngleLocal;
    private Vector3 targetPositionInFormation;
    private SquadImpulseController _squadImpulseController;
    private bool usingNavMesh = false;
    
    private Vector3 lastPosition;
    private Vector3 lastTargetPosition;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        navAgent = GetComponent<NavMeshAgent>();
        _squadImpulseController = GetComponentInParent<SquadImpulseController>();

        navAgent.speed = maxMoveSpeed;
        navAgent.angularSpeed = rotationSpeed;
        navAgent.radius = 0.25f;
        navAgent.acceleration = 75f;
        navAgent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
        navAgent.updateRotation = false;
        navAgent.updateUpAxis = false;
        navAgent.enabled = false;

        if (transform.parent != null)
        {
            Vector3 worldOffset = transform.position - transform.parent.position;
            formationOffsetLocal = Quaternion.Inverse(transform.parent.rotation) * worldOffset;
            formationAngleLocal = transform.eulerAngles.z - transform.parent.eulerAngles.z;
        }
        else
        {
            Debug.LogWarning($"{gameObject.name}: Could not find squad parent object.");
        }

        transform.SetParent(null);
        lastPosition = transform.position;
    }

    void Update()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player")?.transform;
            return;
        }

        Vector3 unitOffsetFromSquadCenter = player.rotation * formationOffsetLocal;
        targetPositionInFormation = player.position + unitOffsetFromSquadCenter;

        //if (_squadImpulseController.IsInImpulse()) return;

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
                DisableNavMesh();

            MoveTowardsFormationPosition();
        }
        
        lastPosition = transform.position;
        lastTargetPosition = targetPositionInFormation;
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
        
        if (debugMode)
        {
            // Calculate actual displacement from last frame
            Vector3 actualMovement = transform.position - lastPosition;
            Vector3 targetMovement = targetPositionInFormation - lastTargetPosition;
            
            Debug.Log($"[PlayerSquadFollower] === FRAME DEBUG ===");
            Debug.Log($"Position: {transform.position}");
            Debug.Log($"Target: {targetPositionInFormation}");
            Debug.Log($"Direction: {directionToTarget}");
            Debug.Log($"Direction.normalized: {directionToTarget.normalized}");
            Debug.Log($"Distance: {distanceToTarget}");
            Debug.Log($"Current Velocity: {rb.linearVelocity}");
            Debug.Log($"Actual Movement: {actualMovement}");
            Debug.Log($"Target Movement: {targetMovement}");
            Debug.Log($"Player Rotation: {player.eulerAngles.z}");
            
            // Check if we're penetrating any colliders
            Collider2D[] overlaps = Physics2D.OverlapCircleAll(transform.position, 0.3f);
            if (overlaps.Length > 1) // More than just self
            {
                Debug.LogWarning($"[PlayerSquadFollower] OVERLAPPING {overlaps.Length} colliders!");
                foreach (var col in overlaps)
                {
                    if (col.gameObject != gameObject)
                        Debug.LogWarning($"  - Overlapping: {col.gameObject.name}");
                }
            }
        }

        MatchFormationAngle();

        if (distanceToTarget > stoppingDistance)
        {
            Vector2 desiredVelocity = directionToTarget.normalized * Mathf.Min(moveSpeed * distanceToTarget, maxMoveSpeed);
            SetVelocity(desiredVelocity);
            
            if (debugMode)
            {
                Debug.Log($"Setting Velocity: {desiredVelocity}");
            }
        }
        else
        {
            SetVelocity(Vector2.zero);
        }
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
        SetRotation(targetAngle: player.eulerAngles.z + formationAngleLocal);

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
    
    void OnDrawGizmos()
    {
        if (!debugMode || player == null) return;
        
        // Draw target position
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(targetPositionInFormation, 0.2f);
        
        // Draw direction to target
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, targetPositionInFormation);
        
        // Draw velocity
        Gizmos.color = Color.red;
        if (rb != null)
            Gizmos.DrawLine(transform.position, transform.position + (Vector3)rb.linearVelocity * 0.5f);
    }
}