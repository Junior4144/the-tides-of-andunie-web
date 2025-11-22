using UnityEngine;
using UnityEngine.AI;

public class SkeletonBossMovementController : MonoBehaviour
{
    [SerializeField] private SkeletonBossAttributes _attributes;

    private Transform _player;
    private NavMeshAgent _agent;
    private Rigidbody2D _rigidbody;
    private ImpulseController _impulseController;

    void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _impulseController = GetComponent<ImpulseController>();

        if (_agent == null)
        {
            Debug.LogError("NavMeshAgent component missing!");
            return;
        }

        _agent.updateRotation = false;
        _agent.updateUpAxis = false;

        if (_attributes != null)
        {
            _agent.speed = _attributes.MovementSpeed;
            _agent.acceleration = _attributes.MovementSpeed * 4f;
        }
        else
        {
            Debug.LogError("PirateAttributes is NULL - agent speed not set!");
        }
    }

    void Update()
    {
        if (_agent == null || !_agent.enabled) return;
        if (!PlayerManager.Instance) return;

        _player = PlayerManager.Instance.transform;

        if (!_agent.isOnNavMesh)
        {
            Debug.LogError($"Agent NOT on NavMesh! Position: {transform.position}");
            return;
        }

        _agent.SetDestination(_player.position);

        if (_impulseController == null || !_impulseController.IsInImpulse())
        {
            RotateTowardsMovementDirection();
        }
    }

    private void RotateTowardsMovementDirection()
    {
        if (_attributes == null) return;

        Vector3 velocity = _agent.velocity;

        if (velocity.sqrMagnitude < 0.01f) return;

        // Calculate target angle based on movement direction
        float targetAngle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg - 90f;
        
        // Get current angle
        float currentAngle = _rigidbody.rotation;
        
        // Smoothly rotate towards target
        float newAngle = Mathf.MoveTowardsAngle(
            currentAngle, 
            targetAngle, 
            _attributes.RotationSpeed * Time.deltaTime
        );
        
        // Apply the rotation
        _rigidbody.SetRotation(newAngle);
    }
}

