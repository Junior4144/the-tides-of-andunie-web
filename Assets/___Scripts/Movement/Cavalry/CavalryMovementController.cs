using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Rigidbody2D))]
public class CavalryMovementController : MonoBehaviour
{
    [SerializeField] private CavalryAttributes _attributes;

    private Transform _player;
    private NavMeshAgent agent;
    private Rigidbody2D _rigidbody;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        _rigidbody = GetComponent<Rigidbody2D>();
        agent.updatePosition = false;
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        agent.speed = _attributes.TurnSpeed;
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
        agent.SetDestination(Player.position);
        UpdateTargetSpeed();
    }

    private void UpdateTargetSpeed()
    {
        float targetSpeed = IsLinedUpForCharge() ? _attributes.ChargeSpeed : _attributes.TurnSpeed;

        agent.speed = Mathf.MoveTowards(
            agent.speed,
            targetSpeed,
            _attributes.Acceleration * Time.deltaTime
        );
    }

    private bool IsLinedUpForCharge()
    {
        Vector3 directionToTarget = (agent.steeringTarget - (Vector3)_rigidbody.position).normalized;
        Vector3 currentForward = transform.up;
        float angleToSteer = Vector3.Angle(currentForward, directionToTarget);
        return angleToSteer <= _attributes.ChargeAngle;
    }

    

    private void ApplySpeedBasedSteering()
    {
        Vector3 steeringDirection = (agent.steeringTarget - (Vector3)_rigidbody.position).normalized;

        if (steeringDirection.sqrMagnitude < 0.01f) return;

        float desiredRotationAngle = AngleOfVectorInDegrees(steeringDirection) - 90f;

        float currentSpeed = _rigidbody.linearVelocity.magnitude;

        float turnAbility = 1.0f - Mathf.InverseLerp(_attributes.MinSpeedForTurning, _attributes.MaxSpeedForTurning, currentSpeed);

        float effectiveTurnSpeed = _attributes.RotationSpeed * turnAbility;

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
