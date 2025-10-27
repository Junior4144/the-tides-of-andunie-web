using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(NavMeshAgent))]
public class PlayerSquadFollower : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float maxMoveSpeed = 20f;
    [SerializeField] private float inFormationThreshold = 0.5f;
    [SerializeField] private float rotationSpeed = 360f;
    [SerializeField] private float navMeshEngageDistance = 4f;
    [SerializeField] private bool debugMode = false;
    

    private Transform player;
    private Rigidbody2D rb;
    private Rigidbody2D playerRb;
    private NavMeshAgent navAgent;

    private Vector3 formationOffsetLocal;
    private float formationAngleLocal;
    private Vector3 targetPositionInFormation;
    private SquadImpulseController _squadImpulseController;
    private bool usingNavMesh = false;
    private bool inFormation = false;
    
    private Vector3 lastPosition;
    private Vector3 lastTargetPosition;

    void Start()
    {
        InitializeComponents();
        ConfigureNavMesh();
        InitializeFormationData();

        transform.SetParent(null);
    }

    void Update()
    {
        if (!TryFindPlayer()) return;
        
        UpdateFormationTarget();
        HandleMovement();
        UpdateDebugTracking();
    }

    private void InitializeComponents()
    {
        rb = GetComponent<Rigidbody2D>();
        navAgent = GetComponent<NavMeshAgent>();
        _squadImpulseController = GetComponentInParent<SquadImpulseController>();
    }
    
    private void ConfigureNavMesh()
    {
        navAgent.speed = maxMoveSpeed;
        navAgent.angularSpeed = rotationSpeed;
        navAgent.radius = 0.25f;
        navAgent.acceleration = 75f;
        navAgent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
        navAgent.updateRotation = false;
        navAgent.updateUpAxis = false;
        navAgent.enabled = false;
    }

    private void InitializeFormationData()
    {
        if (transform.parent == null)
        {
            Debug.LogWarning($"{gameObject.name}: Could not find squad parent object.");
            return;
        }

        Vector3 worldOffset = transform.position - transform.parent.position;
        formationOffsetLocal = Quaternion.Inverse(transform.parent.rotation) * worldOffset;
        formationAngleLocal = transform.eulerAngles.z - transform.parent.eulerAngles.z;

        lastPosition = transform.position;
    }

    private bool TryFindPlayer()
    {
        if (player != null) return true;
        
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
            playerRb = playerObj.GetComponent<Rigidbody2D>();
        }
        
        return player != null;
    }

    private void UpdateFormationTarget()
    {
        Vector3 unitOffsetFromSquadCenter = player.rotation * formationOffsetLocal;
        targetPositionInFormation = player.position + unitOffsetFromSquadCenter;
    }

    private void HandleMovement()
    {
        float distanceToFormation = Vector3.Distance(transform.position, targetPositionInFormation);

        if (distanceToFormation > navMeshEngageDistance)
        {
            HandleNavMeshMovement();
            inFormation = false;
        }
        else if (distanceToFormation <= inFormationThreshold)
        {
            HandleInFormation();
            inFormation = true;
        }
        else
        {
            HandleDirectMovement(distanceToFormation);
            inFormation = false;
        }
    }

    private void HandleNavMeshMovement()
    {
        if (!usingNavMesh) EnableNavMesh();
        
        navAgent.SetDestination(targetPositionInFormation);
        RotateTowardsMovementDirection();
    }

    private void HandleInFormation()
    {
        if (usingNavMesh) DisableNavMesh();
        
        // Lock to formation and match player velocity
        rb.MovePosition(targetPositionInFormation);
        MatchFormationAngle();
        
        if (playerRb != null)
            SetVelocity(playerRb.linearVelocity);
        else
            SetVelocity(Vector2.zero);
    }

    private void HandleDirectMovement(float distance)
    {
        if (usingNavMesh) DisableNavMesh();
        
        MoveTowardsFormationPosition(distance);
    }

    private void UpdateDebugTracking()
    {
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

    private void MoveTowardsFormationPosition(float distanceToTarget)
    {
        Vector3 directionToTarget = (targetPositionInFormation - transform.position).normalized;
        
        if (debugMode) DebugLogs(directionToTarget, distanceToTarget);

        MatchFormationAngle();

        Vector2 desiredVelocity = CalculateDesiredVelocity(directionToTarget, distanceToTarget);
        SetVelocity(desiredVelocity);
    }

    private Vector2 CalculateDesiredVelocity(Vector3 direction, float distance)
    {
        float speedMultiplier = moveSpeed * distance;
        float targetSpeed = Mathf.Min(speedMultiplier, maxMoveSpeed);
        
        return direction * targetSpeed;
    }

    private void RotateTowardsMovementDirection()
    {
        Vector3 velocity = navAgent.velocity;
        
        if (velocity.sqrMagnitude > 0.01f)
        {
            float targetAngle = CalculateAngleFromVelocity(velocity);
            SetRotation(targetAngle);
        }
    }

    private float CalculateAngleFromVelocity(Vector3 velocity) =>
        Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg - 90;

    private void MatchFormationAngle() =>
        SetRotation(targetAngle: player.eulerAngles.z + formationAngleLocal);

    private void SetRotation(float targetAngle)
    {
        float currentAngle = rb.rotation;
        float newAngle = Mathf.MoveTowardsAngle(currentAngle, targetAngle, rotationSpeed * Time.deltaTime);
        rb.SetRotation(newAngle);
    }

    private void SetVelocity(Vector2 velocity) => rb.linearVelocity = velocity;
    
    // TODO: use these logs to debug physics jittering glitch
    private void DebugLogs(Vector2 directionToTarget, float distanceToTarget)
    {
        Vector3 actualMovement = transform.position - lastPosition;
        Vector3 targetMovement = targetPositionInFormation - lastTargetPosition;
        
        Debug.Log($"[PlayerSquadFollower] === FRAME DEBUG ===");
        Debug.Log($"In Formation: {inFormation}");
        Debug.Log($"Position: {transform.position}");
        Debug.Log($"Target: {targetPositionInFormation}");
        Debug.Log($"Direction: {directionToTarget}");
        Debug.Log($"Direction.normalized: {directionToTarget.normalized}");
        Debug.Log($"Distance: {distanceToTarget}");
        Debug.Log($"Current Velocity: {rb.linearVelocity}");
        Debug.Log($"Actual Movement: {actualMovement}");
        Debug.Log($"Target Movement: {targetMovement}");
        Debug.Log($"Player Rotation: {player.eulerAngles.z}");
        
        Collider2D[] overlaps = Physics2D.OverlapCircleAll(transform.position, 0.3f);
        if (overlaps.Length > 1)
        {
            Debug.LogWarning($"[PlayerSquadFollower] OVERLAPPING {overlaps.Length} colliders!");
            foreach (var col in overlaps)
            {
                if (col.gameObject != gameObject)
                    Debug.LogWarning($"  - Overlapping: {col.gameObject.name}");
            }
        }
    }
    
    void OnDrawGizmos()
    {
        if (player == null) return;
        
        // Target position
        Gizmos.color = inFormation ? Color.cyan : Color.green;
        Gizmos.DrawWireSphere(targetPositionInFormation, 0.2f);
        
        // Formation threshold
        Gizmos.color = new Color(0, 1, 1, 0.2f);
        Gizmos.DrawWireSphere(targetPositionInFormation, inFormationThreshold);
        
        // Direction line
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, targetPositionInFormation);
        
        // Velocity indicator
        Gizmos.color = Color.red;
        if (rb != null)
            Gizmos.DrawLine(transform.position, transform.position + (Vector3)rb.linearVelocity * 0.5f);
    }
}