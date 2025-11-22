using UnityEngine;
using UnityEngine.AI;

public class SkeletonBossMovementController : MonoBehaviour
{
    [SerializeField] private SkeletonBossAttributes _attributes;

    private Transform player;
    private NavMeshAgent agent;
    private Rigidbody2D _rigidbody;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        _rigidbody = GetComponent<Rigidbody2D>();

        if (agent == null)
        {
            Debug.LogError("NavMeshAgent component missing!");
            return;
        }

        agent.updateRotation = false;
        agent.updateUpAxis = false;

        if (_attributes != null)
        {
            agent.speed = _attributes.MovementSpeed;
            agent.acceleration = _attributes.MovementSpeed * 4f;
        }
        else
        {
            Debug.LogError("PirateAttributes is NULL - agent speed not set!");
        }
    }

    void Update()
    {
        if (agent == null || !agent.enabled) return;
        if (!PlayerManager.Instance) return;

        player = PlayerManager.Instance.transform;

        if (!agent.isOnNavMesh)
        {
            Debug.LogError($"Agent NOT on NavMesh! Position: {transform.position}");
            return;
        }

        agent.SetDestination(player.position);
        RotateTowardsMovementDirection();
    }

    private void RotateTowardsMovementDirection()
    {
        if (_attributes == null) return;

        Vector3 velocity = agent.velocity;

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

