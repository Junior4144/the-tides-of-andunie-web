using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField]
    private EnemyAttribute _enemyAttribute;


    private Rigidbody2D _rigidbody;
    private PlayerAwarenessController _playerAwarenessController;
    private Vector2 _targetDirection;
    private float _changeDirectionCooldown;
    private Camera _camera;
    private RaycastHit2D[] _obstacleCollisions;
    private float _obstacleAvoidanceCooldown;
    private Vector2 _obstacleAvoidanceTargetDirection;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _playerAwarenessController = GetComponent<PlayerAwarenessController>();
        _targetDirection = transform.up;
        _camera = Camera.main;
        _obstacleCollisions = new RaycastHit2D[10];
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        UpdateTargetDirection();
        RotateTowardsTarget();
        SetVelocity();
    }
    private void UpdateTargetDirection()
    {
        HandleRandomDirectionChange();
        HandlePlayerTargeting();
        HandleObstacle();
        HandleEnemyOffScreen();
    }

    private void HandleRandomDirectionChange()
    {
        _changeDirectionCooldown -= Time.deltaTime;

        if (_changeDirectionCooldown <= 0)
        {
            float angleChange = Random.Range(-90f, 90f);
            Quaternion rotation = Quaternion.AngleAxis(angleChange, transform.forward);
            _targetDirection = rotation * _targetDirection;

            _changeDirectionCooldown = Random.Range(15f, 15f);
        }
    }

    private void HandlePlayerTargeting()
    {
        if (_playerAwarenessController.AwareOfPlayer)
        {
            _targetDirection = _playerAwarenessController.DirectionToPlayer;
        }
    }

    private void HandleEnemyOffScreen()
    {
        Vector2 screenPosition = _camera.WorldToScreenPoint(transform.position);
        //left and right
        if ((screenPosition.x < _enemyAttribute.ScreenBorder && _targetDirection.x < 0) ||
            screenPosition.x > _camera.pixelWidth - _enemyAttribute.ScreenBorder && _targetDirection.x > 0)
        {
            _targetDirection = new Vector2(-_targetDirection.x, _targetDirection.y);
        }

        //bottom and top
        if ((screenPosition.y < _enemyAttribute.ScreenBorder && _targetDirection.y < 0) ||
    screenPosition.y > _camera.pixelHeight - _enemyAttribute.ScreenBorder && _targetDirection.y > 0)
        {
            _targetDirection = new Vector2(_targetDirection.x, -_targetDirection.y);
        }
    }

    private void HandleObstacle()
    {
        _obstacleAvoidanceCooldown -= Time.deltaTime;
        var contactFilter = new ContactFilter2D();
        contactFilter.SetLayerMask(_enemyAttribute.ObstacleLayerMask);

        int numberOfCollisions = Physics2D.CircleCast(
            transform.position,
            _enemyAttribute.ObstacleCheckCircleRadius,
            transform.up,
            contactFilter,
            _obstacleCollisions,
            _enemyAttribute.ObstacleCheckDistance);

        for (int index = 0; index < numberOfCollisions; index++)
        {
            var obstacleCollision = _obstacleCollisions[index];

            if (obstacleCollision.collider.gameObject == gameObject)
            {
                continue;
            }

            if (_obstacleAvoidanceCooldown <= 0)
            {
                _obstacleAvoidanceTargetDirection = obstacleCollision.normal;
                _obstacleAvoidanceCooldown = 0.5f;
            }

            var targetRotation = Quaternion.LookRotation(transform.forward, _obstacleAvoidanceTargetDirection);
            var rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _enemyAttribute.RotationSpeed * Time.deltaTime);


            
            _targetDirection = rotation * Vector2.up;
            break;
        }
    }

    private void RotateTowardsTarget()
    {
        if (_targetDirection == Vector2.zero)
        {
            return;
        }

        Quaternion targetRotation = Quaternion.LookRotation(transform.forward, _targetDirection);
        Quaternion rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _enemyAttribute.RotationSpeed * Time.deltaTime);

        _rigidbody.SetRotation(rotation);

    }
    private void SetVelocity()
    {

        _rigidbody.linearVelocity = transform.up * _enemyAttribute.Speed;

    }

}