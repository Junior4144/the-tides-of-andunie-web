using UnityEngine;
using UnityEngine.AI;

public class PirateMovementController : MonoBehaviour
{
    [SerializeField] private PirateAttributes _attributes;

    private Transform player;
    private NavMeshAgent agent;
    private Rigidbody2D _rigidbody;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        _rigidbody = GetComponent<Rigidbody2D>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        agent.speed = _attributes.MovementSpeed;
        agent.acceleration = _attributes.MovementSpeed * 6f;
        if( _attributes.Acceleration > 5)
        {
            agent.acceleration = _attributes.Acceleration;
        }
    }

    void Update()
    {
        if (!agent.enabled || !PlayerManager.Instance) return;

        player = PlayerManager.Instance.transform;

        RotateTowardsMovementDirection();

        agent.SetDestination(player.position);
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
}
