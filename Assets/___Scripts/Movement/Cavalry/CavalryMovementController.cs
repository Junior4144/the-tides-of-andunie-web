using UnityEngine;
using UnityEngine.AI;
using System.Linq;
using System.Collections.Generic;
using Unity.Properties;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Rigidbody2D))]
public class CavalryMovementController : MonoBehaviour
{
    [SerializeField] private CavalryAttributes _attributes;

    private Transform _player;
    private NavMeshAgent agent;
    private Rigidbody2D _rigidbody;

    private enum CavalryState { Patrolling, Attacking }
    private CavalryState _currentState;
    private bool _hasHitThePlayer;

    [SerializeField] private List<Transform> PatrolPointsSequence;
    private int _currentPatrolPointIndex;
    private NavMeshPath _pathPlaceholder;

    private float _lastAttackTime;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        _rigidbody = GetComponent<Rigidbody2D>();
        agent.updatePosition = false;
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        _pathPlaceholder = new NavMeshPath();
        _currentPatrolPointIndex = 0;
        TransitionToPatrollingState();
        _hasHitThePlayer = false;
        _lastAttackTime = -_attributes.AttackCoolDown;
    }

    private Transform Player => _player == null ? _player = PlayerManager.Instance.transform : _player;

    void FixedUpdate()
    {
        if (!agent.enabled) return;
        
        SyncAgentToRigidbody();
        ApplyForwardMovement();
        ApplySpeedBasedSteering();
    }

    void SyncAgentToRigidbody() => agent.nextPosition = _rigidbody.position;
    void ApplyForwardMovement() => _rigidbody.linearVelocity = transform.up * agent.desiredVelocity.magnitude;

    void Update()
    {
        
        if (!agent.enabled || Player == null) return;

        switch (_currentState)
        {
            case CavalryState.Patrolling:
                bool canAttack = Time.time - _lastAttackTime > _attributes.AttackCoolDown;
                if (canAttack && IsLinedUpWithTarget(Player.transform.position) && IsStraightPathToPlayer())
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

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (_currentState == CavalryState.Attacking && collision.gameObject.CompareTag("Player"))
            HandlePlayerHit();
        
    }

    private void HandlePlayerHit()
    {
        _hasHitThePlayer = true;
        _lastAttackTime = Time.time;
    }

    private void Patrol()
    {
        Vector3 CurrentPatrolPointPosition() => PatrolPointsSequence[_currentPatrolPointIndex].position;
        ChangeAgentSpeedTowards(_attributes.PatrollingSpeed);
        if (CalculateDisplacementToTarget(CurrentPatrolPointPosition()) <= 1.0f)
        {
            _currentPatrolPointIndex = 
            _currentPatrolPointIndex + 1 == PatrolPointsSequence.Count()?
            0 : _currentPatrolPointIndex + 1;
        }
        agent.SetDestination(CurrentPatrolPointPosition());
    }

    private void ChasePlayer()
    {
        agent.SetDestination(Player.position);
        UpdateChaseSpeed();
    }

    
    private void TransitionToAttackingState()
    {
        Debug.Log("Cavalry Attacking!");
        _currentState = CavalryState.Attacking;
    }

    private void TransitionToPatrollingState()
    {
        Debug.Log("Cavalry Patrolling...");
        _currentState = CavalryState.Patrolling;
        _hasHitThePlayer = false;
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
        if (PatrolPointsSequence == null || PatrolPointsSequence.Count == 0){
            Debug.LogError("No PatrolPoints assigned to Cavalry unit.");
             return;
        }   

        int nearestIndex = -1;
        float minDistance = Mathf.Infinity;

        for (int i = 0; i < PatrolPointsSequence.Count; i++)
        {
            float distance = CalculateDistanceToTarget(PatrolPointsSequence[i].position);

            if (distance < minDistance)
            {
                minDistance = distance;
                nearestIndex = i;
            }
        }

        _currentPatrolPointIndex = nearestIndex;
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

        if (pathDistance == Mathf.Infinity || pathDistance < 0.01f)
            return false;

        float straightness = displacement / pathDistance;
        return straightness >= _attributes.MinPathStraightnessToAttack;
    }
    private void ApplySpeedBasedSteering()
    {
        Vector3 steeringDirection = (agent.steeringTarget - (Vector3)_rigidbody.position).normalized;

        if (steeringDirection.sqrMagnitude < 0.01f) return;

        float desiredRotationAngle = AngleOfVectorInDegrees(steeringDirection) - 90f;

        float currentSpeed = _rigidbody.linearVelocity.magnitude;

        float turnAbility = 1.0f - Mathf.InverseLerp(_attributes.MinSpeedForTurning, _attributes.MaxSpeedForTurning, currentSpeed);

        float effectiveTurnSpeed = _attributes.TurningSpeed * turnAbility;

        _rigidbody.SetRotation(
            Mathf.MoveTowardsAngle(
                _rigidbody.rotation,
                desiredRotationAngle,
                effectiveTurnSpeed * Time.fixedDeltaTime 
            )
        );
    }

    private float AngleOfVectorInDegrees(Vector3 vector)
     => Mathf.Atan2(vector.y, vector.x) * Mathf.Rad2Deg;

}
