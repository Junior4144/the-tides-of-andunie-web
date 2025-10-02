using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private Rigidbody2D _rigidbody;
    private PlayerAwarenessController _playerAwarenessController;
    private Vector2 _targetDirection;
    private RaycastHit2D[] _obstacleCollisions;
    private float _obstacleAvoidanceCooldown;
    private Vector2 _obstacleAvoidanceTargetDirection;
    private Impulse impulseScript;

    [SerializeField]
    private EnemyAttribute _enemyAttribute;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _playerAwarenessController = GetComponent<PlayerAwarenessController>();
        impulseScript = GetComponentInChildren<Impulse>();
        _targetDirection = transform.up;
        _obstacleCollisions = new RaycastHit2D[10];
    }

    private void FixedUpdate()
    {
        UpdateTargetDirection();
        RotateTowardsTarget();
        SetVelocity();
    }

    private void UpdateTargetDirection()
    {
        HandlePlayerTargeting();
        HandleObstacle();
    }

    private void HandlePlayerTargeting()
    {
        if (_playerAwarenessController.AwareOfPlayer)
            _targetDirection = _playerAwarenessController.DirectionToPlayer;
    }

    private void HandleObstacle()
    {
        UpdateCooldown();

        int numberOfCollisions = DetectObstacles(out var collisions);

        for (int i = 0; i < numberOfCollisions; i++)
        {
            if (IsSelfCollision(collisions[i])) continue;

            if (CanAvoid())
            {
                SetAvoidanceDirection(collisions[i]);
                RotateTowardsAvoidance();
                break; 
            }
        }
    }
    private void UpdateCooldown() =>
        _obstacleAvoidanceCooldown -= Time.deltaTime;
    private int DetectObstacles(out RaycastHit2D[] collisions)
    {
        var contactFilter = new ContactFilter2D();
        contactFilter.SetLayerMask(_enemyAttribute.ObstacleLayerMask);

        int hits = Physics2D.CircleCast(
            transform.position,
            _enemyAttribute.ObstacleCheckCircleRadius,
            transform.up,
            contactFilter,
            _obstacleCollisions,
            _enemyAttribute.ObstacleCheckDistance);

        collisions = _obstacleCollisions;
        return hits;
    }
    private bool IsSelfCollision(RaycastHit2D collisions) =>
        collisions.collider.gameObject == gameObject;

    private bool CanAvoid() =>
        _obstacleAvoidanceCooldown <= 0;

    private void SetAvoidanceDirection(RaycastHit2D collisions)
    {
        _obstacleAvoidanceTargetDirection = collisions.normal;
        _obstacleAvoidanceCooldown = 0.5f;
    }

    private void RotateTowardsAvoidance()
    {
        var targetRotation = Quaternion.LookRotation(
            transform.forward, 
            _obstacleAvoidanceTargetDirection
            );
        var rotation = Quaternion.RotateTowards(
            transform.rotation, targetRotation, 
            _enemyAttribute.RotationSpeed * Time.deltaTime
            );

        _targetDirection = rotation * Vector2.up;
    }

    private void RotateTowardsTarget()
    {
        if (_targetDirection == Vector2.zero) return;

        Quaternion targetRotation = Quaternion.LookRotation(transform.forward, _targetDirection);
        Quaternion rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _enemyAttribute.RotationSpeed * Time.deltaTime);

        _rigidbody.SetRotation(rotation);
    }

    private void SetVelocity()
    {
        if (impulseScript != null && impulseScript.IsInImpulse()) return;

        _rigidbody.linearVelocity = transform.up * _enemyAttribute.Speed;
    }
}