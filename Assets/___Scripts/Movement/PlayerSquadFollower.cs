using System.Collections;
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
    

    private Transform _player;
    private Rigidbody2D _rb;
    private Rigidbody2D _playerRb;
    private NavMeshAgent _navAgent;
    private PlayerHeroMovement _playerHeroMovement;

    private Vector3 _formationOffsetLocal;
    private float _formationAngleLocal;
    private Vector3 _targetPositionInFormation;
    private PlayerSquadImpulseController _squadImpulseController;
    private bool _usingNavMesh = false;
    private bool _isInFormation = false;
    private bool _isDashing = false;
    private bool _offsetInitialized = false;

    private Vector3 lastPosition;
    private Vector3 lastTargetPosition;

    public void Initialize(Vector2 formationOffset)
    {
        _formationOffsetLocal = formationOffset;
        _formationAngleLocal = 0f;
        _offsetInitialized = true;
        lastPosition = transform.position;

        Debug.Log($"[PlayerSquadFollower] Offset initialized {_formationOffsetLocal}");
    }

    void Start()
    {
        InitializeComponents();
        ConfigureNavMesh();

        if (!_offsetInitialized)
            Debug.LogWarning($"[PlayerSquadFollower] Offset not initialized for {gameObject.name}");

        if (_player != null)
        {
            UpdateFormationTarget();
            Debug.Log($"[PlayerSquadFollower] Initial target position {_targetPositionInFormation}");
        }
    }

    void OnDestroy()
    {
        if (_playerHeroMovement != null)
            _playerHeroMovement.OnPlayerDash -= HandlePlayerDash;
    }

    void Update()
    {
        if (!_player) return;
        if (_isDashing) return;
        if (_squadImpulseController.IsInImpulse()) return;

        UpdateFormationTarget();
        HandleMovement();
        UpdateDebugTracking();
    }

    private void InitializeComponents()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");

        if (playerObj != null)
        {
            _player = playerObj.transform;
            _playerRb = playerObj.GetComponent<Rigidbody2D>();
            _playerHeroMovement = playerObj.GetComponent<PlayerHeroMovement>();
            _squadImpulseController = playerObj.GetComponent<PlayerSquadImpulseController>();

            if (_playerHeroMovement != null)
                _playerHeroMovement.OnPlayerDash += HandlePlayerDash;
        }

        _rb = GetComponent<Rigidbody2D>();
        _navAgent = GetComponent<NavMeshAgent>();
        
    }
    
    private void ConfigureNavMesh()
    {
        _navAgent.speed = maxMoveSpeed;
        _navAgent.angularSpeed = rotationSpeed;
        _navAgent.radius = 0.25f;
        _navAgent.acceleration = 75f;
        _navAgent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
        _navAgent.updateRotation = false;
        _navAgent.updateUpAxis = false;
        _navAgent.enabled = false;
    }

    private void UpdateFormationTarget()
    {
        Vector3 unitOffsetFromSquadCenter = _player.rotation * _formationOffsetLocal;
        _targetPositionInFormation = _player.position + unitOffsetFromSquadCenter;
    }

    private void HandleMovement()
    {
        float distanceToFormation = Vector3.Distance(transform.position, _targetPositionInFormation);

        if (distanceToFormation > navMeshEngageDistance)
        {
            HandleNavMeshMovement();
            _isInFormation = false;
        }
        else if (distanceToFormation <= inFormationThreshold)
        {
            HandleInFormation();
            _isInFormation = true;
        }
        else
        {
            HandleDirectMovement(distanceToFormation);
            _isInFormation = false;
        }
    }

    private void HandleNavMeshMovement()
    {
        if (!_usingNavMesh) EnableNavMesh();
        
        _navAgent.SetDestination(_targetPositionInFormation);
        RotateTowardsMovementDirection();
    }

    private void HandleInFormation()
    {
        if (_usingNavMesh) DisableNavMesh();

        _rb.MovePosition(_targetPositionInFormation);
        MatchFormationAngle();
        
        if (_playerRb != null)
            SetVelocity(_playerRb.linearVelocity);
        else
            SetVelocity(Vector2.zero);
    }

    private void HandleDirectMovement(float distance)
    {
        if (_usingNavMesh) DisableNavMesh();
        
        MoveTowardsFormationPosition(distance);
    }

    private void UpdateDebugTracking()
    {
        lastPosition = transform.position;
        lastTargetPosition = _targetPositionInFormation;
    }

    private void EnableNavMesh()
    {
        _usingNavMesh = true;
        _navAgent.enabled = true;
    }

    private void DisableNavMesh()
    {
        _usingNavMesh = false;
        _navAgent.enabled = false;
    }

    private void MoveTowardsFormationPosition(float distanceToTarget)
    {
        Vector3 directionToTarget = (_targetPositionInFormation - transform.position).normalized;
        
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
        Vector3 velocity = _navAgent.velocity;
        
        if (velocity.sqrMagnitude > 0.01f)
        {
            float targetAngle = CalculateAngleFromVelocity(velocity);
            SetRotation(targetAngle);
        }
    }

    private float CalculateAngleFromVelocity(Vector3 velocity) =>
        Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg - 90;

    private void MatchFormationAngle() =>
        SetRotation(targetAngle: _player.eulerAngles.z + _formationAngleLocal);

    private void SetRotation(float targetAngle)
    {
        float currentAngle = _rb.rotation;
        float newAngle = Mathf.MoveTowardsAngle(currentAngle, targetAngle, rotationSpeed * Time.deltaTime);
        _rb.SetRotation(newAngle);
    }

    private void SetVelocity(Vector2 velocity) => _rb.linearVelocity = velocity;

    private void HandlePlayerDash(Vector2 dashDirection, float dashForce, float dashDuration) =>
        StartCoroutine(DashCoroutine(dashForce, dashDuration));

    private IEnumerator DashCoroutine(float dashForce, float dashDuration)
    {
        _isDashing = true;

        if (_usingNavMesh) DisableNavMesh();

        _rb.linearVelocity = Vector2.zero;
        _rb.AddForce(transform.up * dashForce, ForceMode2D.Impulse);

        yield return new WaitForSeconds(dashDuration);
        _isDashing = false;
    }

    private void DebugLogs(Vector2 directionToTarget, float distanceToTarget)
    {
        Vector3 actualMovement = transform.position - lastPosition;
        Vector3 targetMovement = _targetPositionInFormation - lastTargetPosition;
        
        Debug.Log($"[PlayerSquadFollower] === FRAME DEBUG ===");
        Debug.Log($"In Formation: {_isInFormation}");
        Debug.Log($"Position: {transform.position}");
        Debug.Log($"Target: {_targetPositionInFormation}");
        Debug.Log($"Direction: {directionToTarget}");
        Debug.Log($"Direction.normalized: {directionToTarget.normalized}");
        Debug.Log($"Distance: {distanceToTarget}");
        Debug.Log($"Current Velocity: {_rb.linearVelocity}");
        Debug.Log($"Actual Movement: {actualMovement}");
        Debug.Log($"Target Movement: {targetMovement}");
        Debug.Log($"Player Rotation: {_player.eulerAngles.z}");
        
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
        if (_player == null) return;
        
        Gizmos.color = _isInFormation ? Color.cyan : Color.green;
        Gizmos.DrawWireSphere(_targetPositionInFormation, 0.2f);
        
        Gizmos.color = new Color(0, 1, 1, 0.2f);
        Gizmos.DrawWireSphere(_targetPositionInFormation, inFormationThreshold);
        
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, _targetPositionInFormation);
        
        Gizmos.color = Color.red;
        if (_rb != null)
            Gizmos.DrawLine(transform.position, transform.position + (Vector3)_rb.linearVelocity * 0.5f);
    }
}