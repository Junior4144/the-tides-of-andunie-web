using UnityEngine;
using UnityEngine.AI;
using System.Linq;
using System.Collections.Generic;
using System;
// using Unity.Properties;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Rigidbody2D))]
public class CavalryMovementController : MonoBehaviour
{
    // External objects and variables
    [SerializeField] private CavalryAttributes _attributes;
    [SerializeField] private bool _reversePatrolOrder;
    private Transform _player;
    private NavMeshAgent agent;
    private Rigidbody2D _rigidbody;

    private CavalryMeleeController _meleeController;

    // State variables
    private enum CavalryState { Patrolling, Attacking, Stuck }
    private CavalryState _currentState;
    private bool _hasHitThePlayer;

    // Events
    public event System.Action OnChargeStart;

    // Public getters
    public float CurrentSpeed => agent != null ? agent.speed : 0f;
    public float MaxSpeed => _attributes.ChargeSpeed;

    // Path finding variables
    private List<Transform> _patrolPointsSequence;
    private int _currentPatrolPointIndex;
    private NavMeshPath _pathPlaceholder;

    // Stuck detection fields
    private Vector3 _lastPosition;
    private float _lastMovementTime;
    private bool _isStuck;
    private float _backupStartTime;
    private float _backupDuration = 1f;

    // Local getters
    private Transform Player => _player ??= PlayerManager.Instance.transform;
    private float TurnAbility => 1.0f - Mathf.InverseLerp(_attributes.MinSpeedForTurning, _attributes.MaxSpeedForTurning, _rigidbody.linearVelocity.magnitude);
    private float EffectiveTurnSpeed => _attributes.TurningSpeed * TurnAbility;

    void Awake()
    {
        AssignPatrolPointsSequence();
        agent = GetComponent<NavMeshAgent>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _meleeController = gameObject.GetComponentInChildren<CavalryMeleeController>();

        agent.updatePosition = false;
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        _pathPlaceholder = new NavMeshPath();
        
        _lastPosition = _rigidbody.position;
        _lastMovementTime = Time.time;
        _isStuck = false;
        _backupStartTime = 0f;

        SetNearestPatrolPoint();
        TransitionToPatrollingState();
        _hasHitThePlayer = false;
    }

    void OnEnable()
    {
        _meleeController.OnAttack += HandlePlayerHit;
    }

    void OnDisable()
    {
        _meleeController.OnAttack -= HandlePlayerHit;
    }

    void AssignPatrolPointsSequence()
    {
        var patrolPointsSets = GameObject.FindGameObjectsWithTag("CavalryPatrolPointsSet");
        if (patrolPointsSets.Length == 0)
        {
            Debug.LogError("No PatrolPointsSets found!");
            return;
        }
        var patrolPointsSetWithLeastUnits = patrolPointsSets.OrderBy(set => set.GetComponent<CavalryPatrolPointsController>().GetCurrentPatrollingUnits()).First();
        _patrolPointsSequence = patrolPointsSetWithLeastUnits.GetComponent<CavalryPatrolPointsController>().SubscribeToPatrolPointSequence(gameObject);
    }

    void FixedUpdate()
    {
        if (!agent.enabled) return;
        
        CheckAndHandleStuck();
        
        if (!_isStuck) SetDefaultMovement();
        SyncAgentToRigidbody();
    }

    void SyncAgentToRigidbody() => agent.nextPosition = _rigidbody.position;
    void ApplyForwardMovement() => _rigidbody.linearVelocity = transform.up * agent.desiredVelocity.magnitude;

    void Update()
    {
        if (!agent.enabled || Player == null) return;

        switch (_currentState)
        {
            case CavalryState.Patrolling:
                if (_meleeController.CanAttack && IsLinedUpWithTarget(Player.position) && IsStraightPathToPlayer())
                    TransitionToAttackingState();
                else
                    Patrol();
                break;

            case CavalryState.Attacking:
                if (!IsLinedUpWithTarget(Player.transform.position) || !IsStraightPathToPlayer() || _hasHitThePlayer)
                {
                    SetNearestPatrolPoint();
                    TransitionToPatrollingState();
                }
                else 
                    ChasePlayer();
                break;
        }
    }

    private void HandlePlayerHit()
    {
        _hasHitThePlayer = true;
    }

    private void Patrol()
    {
        Vector3 CurrentPatrolPointPosition() => _patrolPointsSequence[_currentPatrolPointIndex].position;
        ChangeAgentSpeedTowards(_attributes.PatrollingSpeed);
        if (CalculateDisplacementToTarget(CurrentPatrolPointPosition()) <= _attributes.PatrolPointReachedThreshold)
        {
            _currentPatrolPointIndex = NextPatrolPointIndex();
        }
        agent.SetDestination(CurrentPatrolPointPosition());
    }

    private void ChasePlayer()
    {
        agent.SetDestination(Player.position);
        UpdateChaseSpeed();
    }

    private int NextPatrolPointIndex()
    {
        int direction = _reversePatrolOrder ? -1 : 1;
        int nextIndex =_currentPatrolPointIndex + direction;
        if (nextIndex < 0) nextIndex = _patrolPointsSequence.Count - 1;
        else if (nextIndex == _patrolPointsSequence.Count) nextIndex = 0;
        return nextIndex;
    }
    private void ReversePatrolPointSequence() => _reversePatrolOrder = !_reversePatrolOrder;
    
    private void TransitionToAttackingState()
    {
        _currentState = CavalryState.Attacking;
        OnChargeStart?.Invoke();
    }

    private void TransitionToPatrollingState()
    {
        _currentState = CavalryState.Patrolling;
        _hasHitThePlayer = false;
        SetDefaultMovement();
    }

    private float CalculateDistanceToTarget(Vector3 targetPosition)
    {
        if (!agent.CalculatePath(targetPosition, _pathPlaceholder) || _pathPlaceholder.status != NavMeshPathStatus.PathComplete)
        {
            return Mathf.Infinity;
        }

        float totalDistance = _pathPlaceholder.corners.Zip(_pathPlaceholder.corners.Skip(1), Vector3.Distance).Sum();

        return totalDistance;
    }

    private float CalculateDisplacementToTarget(Vector3 targetPosition) => Vector3.Distance(_rigidbody.position, targetPosition);

    private void SetNearestPatrolPoint()
    {
        if (_patrolPointsSequence == null || _patrolPointsSequence.Count == 0){
            Debug.LogError("No PatrolPoints assigned to Cavalry unit.");
             return;
        }   

        int nearestIndex = -1;
        float minDistance = Mathf.Infinity;

        for (int i = 0; i < _patrolPointsSequence.Count; i++)
        {
            float distance = CalculateWeightedDistanceToTarget(_patrolPointsSequence[i].position);

            if (distance < minDistance)
            {
                minDistance = distance;
                nearestIndex = i;
            }
        }

        _currentPatrolPointIndex = nearestIndex;

        if (_patrolPointsSequence.Count > 2 && PreviousPatrolAngleIsLess())
            ReversePatrolPointSequence();
    }

    private bool PreviousPatrolAngleIsLess()
    {
        Vector3 currentPoint = _patrolPointsSequence[_currentPatrolPointIndex].position;
        
        int nextIndex = (_currentPatrolPointIndex + 1) % _patrolPointsSequence.Count;
        Vector3 nextPoint = _patrolPointsSequence[nextIndex].position;

        int prevIndex = _currentPatrolPointIndex - 1;
        if (prevIndex < 0) prevIndex = _patrolPointsSequence.Count - 1;
        Vector3 prevPoint = _patrolPointsSequence[prevIndex].position;

        Vector3 forwardPatrolDir = (nextPoint - currentPoint).normalized;
        Vector3 backwardPatrolDir = (prevPoint - currentPoint).normalized;
        
        Vector3 currentFacingDir = transform.up;

        float angleToForward = Vector3.Angle(currentFacingDir, forwardPatrolDir);
        float angleToBackward = Vector3.Angle(currentFacingDir, backwardPatrolDir);


        return angleToBackward < angleToForward;
    }

    private float CalculateWeightedDistanceToTarget(Vector3 targetPosition)
    {
        float pathDistance = CalculateDistanceToTarget(targetPosition);
        
        Vector3 currentPosition = (Vector3)_rigidbody.position;
        Vector3 directionToPoint = (targetPosition - currentPosition).normalized;
        Vector3 currentForward = transform.up;
        
        float angleToPoint = Vector3.Angle(currentForward, directionToPoint);
        
        float behindnessPenalty = 1f;
        if (angleToPoint > _attributes.ChargeAngle)
        {
            float behindnessFactor = (angleToPoint - 90f) / 90f;
            float penaltyMultiplier = 100f;
            behindnessPenalty = behindnessFactor * _attributes.TargetBehindnessPenalty * penaltyMultiplier;
        }
        
        return pathDistance * behindnessPenalty;
    }

    private void UpdateChaseSpeed()
    {
        float targetSpeed = IsLinedUpWithTarget(Player.transform.position) ? _attributes.ChargeSpeed : _attributes.PatrollingSpeed;
        ChangeAgentSpeedTowards(targetSpeed, _attributes.ChargeAcceleration);
    }

    private void ChangeAgentSpeedTowards(float targetSpeed, float rate = 0.0f){
        float currentSpeed = agent.speed;
        
        if (rate == 0) rate = currentSpeed < targetSpeed ?
        _attributes.Acceleration : _attributes.Deceleration;

        
        agent.speed = Mathf.MoveTowards(
            agent.speed,
            targetSpeed,
            rate * Time.deltaTime
        );
    }

    private bool IsLinedUpWithTarget(Vector3 targetPosition)
    {
        Vector3 directionToTarget = (targetPosition - (Vector3)_rigidbody.position).normalized;
        Vector3 currentForward = transform.up;
        float angleToTarget = Vector3.Angle(currentForward, directionToTarget);
        return angleToTarget <= _attributes.ChargeAngle;
    }


    private bool IsStraightPathToPlayer()
    {
        float displacement = CalculateDisplacementToTarget(Player.position);
        float pathDistance = CalculateDistanceToTarget(Player.position);

        if (pathDistance == Mathf.Infinity) return false;
        if (displacement < _attributes.PatrolPointReachedThreshold) return true;

        float straightness = displacement / pathDistance;
        return straightness >= _attributes.MinPathStraightnessToAttack;
    }

   
    private void ApplySpeedBasedSteering()
    {
        Vector3 steeringDirection = (agent.steeringTarget - (Vector3)_rigidbody.position).normalized;

        if (steeringDirection.sqrMagnitude < 0.01f) return;

        float desiredRotationAngle = AngleOfVectorInDegrees(steeringDirection) - 90f;

        _rigidbody.SetRotation(
            Mathf.MoveTowardsAngle(
                _rigidbody.rotation,
                desiredRotationAngle,
                EffectiveTurnSpeed * Time.fixedDeltaTime 
            )
        );
    }

    private float AngleOfVectorInDegrees(Vector3 vector)
     => Mathf.Atan2(vector.y, vector.x) * Mathf.Rad2Deg;



    private void CheckAndHandleStuck()
    {
        if (_isStuck){
            if (Time.time - _backupStartTime < _backupDuration)
            {
                Debug.Log($"cavalry is stuck while going to {_patrolPointsSequence[_currentPatrolPointIndex]}");
                SetBackwardsMovement();
            }
            else
            {
                _isStuck = false;
                TransitionToPatrollingState();
            }
        }
        
        float currentSpeed = _rigidbody.linearVelocity.magnitude;
        float distanceMoved = Vector3.Distance(_rigidbody.position, _lastPosition);
        
        if (currentSpeed > 0.1f || distanceMoved > 0.01f)
        {
            _lastPosition = _rigidbody.position;
            _lastMovementTime = Time.time;
            return;
        }
        
        // Check if stuck (hasn't moved for the threshold time)
        if (Time.time - _lastMovementTime >= _attributes.StuckDetectionTime)
        {
            TransitionToStuckState();
        }
    }

    private void TransitionToStuckState()
    {
        _currentState = CavalryState.Stuck;
        _isStuck = true;
        _backupStartTime = Time.time;
        SetBackwardsMovement();
    }


    private void SetBackwardsMovement()
    {
        _rigidbody.linearVelocity = -transform.up * (_attributes.PatrollingSpeed * 0.5f);
    }

    private void SetDefaultMovement()
    {
        ApplyForwardMovement();
        ApplySpeedBasedSteering();
    }
}
