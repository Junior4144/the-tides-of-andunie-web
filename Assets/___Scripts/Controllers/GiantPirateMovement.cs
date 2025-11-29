using UnityEngine;
using UnityEngine.AI;

public class GiantPirateMovement : MonoBehaviour
{
    [SerializeField] private PirateAttributes _attributes;
    [SerializeField] private float playerVelocityThreshold = 1.5f;
    private Transform player;
    private NavMeshAgent agent;
    private Rigidbody2D _rigidbody;
    private Rigidbody2D playerRb;


    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        _rigidbody = GetComponent<Rigidbody2D>();

        agent.updateRotation = false;
        agent.updateUpAxis = false;

        agent.speed = _attributes.MovementSpeed;
        agent.acceleration = _attributes.MovementSpeed * 6f;

        if (_attributes.Acceleration > 5)
            agent.acceleration = _attributes.Acceleration;
    }

    void Start()
    {
        // Get player transform + rigidbody once
        player = PlayerManager.Instance.GetPlayerTransform();
        playerRb = player.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (!agent.enabled || !player) return;

        agent.SetDestination(player.position);

        HandleRotation();
    }

    private void HandleRotation()
    {
        if (playerRb == null)
        {
            // fallback to old behavior
            RotateTowardsMovementDirection();
            return;
        }

        float pirateSpeed = agent.velocity.magnitude;

        if (pirateSpeed > playerVelocityThreshold)
        {
            // Player is moving → Rotate using movement direction
            RotateTowardsMovementDirection();
        }
        else
        {
            // Player is slow/idle → Rotate to look at the player
            RotateTowardsPlayer();
        }
    }

    private void RotateTowardsMovementDirection()
    {
        Vector3 velocity = agent.velocity;

        if (velocity.sqrMagnitude < 0.01f) return;

        float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(0, 0, angle - 90f);

        _rigidbody.SetRotation(
            Mathf.MoveTowardsAngle(
                _rigidbody.rotation,
                targetRotation.eulerAngles.z,
                _attributes.RotationSpeed * Time.deltaTime
            )
        );
    }

    private void RotateTowardsPlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;

        if (direction.sqrMagnitude < 0.01f) return;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        Quaternion targetRotation = Quaternion.Euler(0, 0, angle - 90f);

        _rigidbody.SetRotation(
            Mathf.MoveTowardsAngle(
                _rigidbody.rotation,
                targetRotation.eulerAngles.z,
                _attributes.RotationSpeed * Time.deltaTime
            )
        );
    }
}