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
        
        agent.nextPosition = _rigidbody.position;
        float desiredSpeed = agent.desiredVelocity.magnitude;

        _rigidbody.linearVelocity = transform.up * desiredSpeed;
        RotateTowardsSteeringTarget();
    }
    void Update()
    {
        if (!agent.enabled || Player == null) return;
        agent.SetDestination(Player.position);
        AdjustSpeed();
    }

    private void AdjustSpeed()
    {
        Vector3 directionToTarget = (agent.steeringTarget - (Vector3)_rigidbody.position).normalized;
        Vector3 currentForward = transform.up;

        float angleToSteer = Vector3.Angle(currentForward, directionToTarget);

        float targetSpeed = angleToSteer <= _attributes.ChargeAngle ? _attributes.ChargeSpeed : _attributes.TurnSpeed;

        agent.speed = Mathf.MoveTowards(
            agent.speed,
            targetSpeed,
            _attributes.Acceleration * Time.deltaTime
        );
    }

    private void RotateTowardsSteeringTarget()
    {
        Vector3 steeringDirection = (agent.steeringTarget - (Vector3)_rigidbody.position).normalized;

        if (steeringDirection.sqrMagnitude < 0.01f) return;

        float rawSteeringAngle = Mathf.Atan2(steeringDirection.y, steeringDirection.x) * Mathf.Rad2Deg;
        float desiredRotation = rawSteeringAngle - 90f;

        float currentSpeed = _rigidbody.linearVelocity.magnitude;

        float turnAbility = 1.0f - Mathf.InverseLerp(_attributes.MinSpeedForTurning, _attributes.MaxSpeedForTurning, currentSpeed);

        float effectiveTurnSpeed = _attributes.RotationSpeed * turnAbility;


        _rigidbody.SetRotation(
            Mathf.MoveTowardsAngle(
                _rigidbody.rotation,
                desiredRotation,
                effectiveTurnSpeed * Time.fixedDeltaTime 
            )
        );
    }
}
