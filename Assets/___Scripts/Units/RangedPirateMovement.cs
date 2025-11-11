using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class RangedPirateMovement : MonoBehaviour
{
    [SerializeField] private float waitTime = 2f;
    [SerializeField] private float awarenessDistance = 10f;
    [SerializeField] private PirateAttributes _attributes;

    [SerializeField] private GameObject ProjectilePrefab;

    private Transform player;
    private int currentPointIndex = 0;
    private NavMeshAgent agent;
    private float waitTimer;
    private Transform currentPatrolPoint;
    private Rigidbody2D _rigidbody;
    private bool canFire = true;   // fire rate control
    public float fireCooldown = 1f; // seconds between shots

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        _rigidbody = GetComponent<Rigidbody2D>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }


    void Update()
    {
        if (!agent.enabled || !PlayerManager.Instance) return;

        player = PlayerManager.Instance.transform;
        RotateTowardsPlayer();
        
        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= 10f && canFire)
        {
            StartCoroutine(initiateFiringSequence());
            return;
        }

        RotateTowardsMovementDirection();
        agent.SetDestination(player.position);
    }

    private IEnumerator initiateFiringSequence()
    {
        canFire = false;
        agent.isStopped = true;

        Instantiate(ProjectilePrefab, transform.position, transform.rotation);

        yield return new WaitForSeconds(fireCooldown); // cooldown between shots
        agent.isStopped = false;
        canFire = true;
    }
    private void RotateTowardsMovementDirection()
    {
        Vector3 velocity = agent.velocity;

        // If agent is stationary, don’t rotate
        if (velocity.sqrMagnitude < 0.01f) return;

        // Calculate facing angle from movement direction
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
