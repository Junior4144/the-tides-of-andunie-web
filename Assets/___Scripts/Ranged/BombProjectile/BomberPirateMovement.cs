using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class BomberPirateMovement : MonoBehaviour
{
    [SerializeField] private float _attackAnimDuration = .9f;
    [SerializeField] private BomberPirateAttributes _attributes;
    [SerializeField] private GameObject ProjectilePrefab;
    [SerializeField] private GameObject firePoint;
    [SerializeField] private AudioClip fireShotSound;

    [Header("Runaway Settings")]
    [SerializeField] private LayerMask obstacleLayerMask;
    [SerializeField] private float runBackTimeout = 1f;

    private Transform player;
    private NavMeshAgent agent;
    private Rigidbody2D _rigidbody;
    private ArcherPirateAnimator _animator;
    private AudioSource _audioSource;
    private ImpulseController _impulseController;

    private bool canFire = true;
    public float fireCooldown = 1f;

    private float originalSpeed;
    private bool isRunningAway = false;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponentInChildren<ArcherPirateAnimator>();
        _audioSource = GetComponent<AudioSource>();
        _impulseController = GetComponentInChildren<ImpulseController>();

        agent.updateRotation = false;
        agent.updateUpAxis = false;

        originalSpeed = agent.speed;
    }

    void Update()
    {
        if (!agent.enabled || !PlayerManager.Instance)
            return;

        player = PlayerManager.Instance.transform;

        if (isRunningAway)
        {
            RotateTowardsMovementDirection();
            return;
        }

        HandleAttackState();
    }

    private void HandleAttackState()
    {
        RotateTowardsPlayer();

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= _attributes.ReadyDistance && canFire && HasLineOfSight())
        {
            StartCoroutine(FireSequence());
            PlayAttackAnimation();
            return;
        }

        if (distance > _attributes.ReadyDistance || !HasLineOfSight())
        {
            RotateTowardsMovementDirection();
            agent.SetDestination(player.position);
        }
    }

    private IEnumerator FireSequence()
    {
        canFire = false;
        agent.isStopped = true;

        yield return new WaitForSeconds(_attackAnimDuration);

        Instantiate(ProjectilePrefab, firePoint.transform.position, transform.rotation);

        if (fireShotSound)
            _audioSource.PlayOneShot(fireShotSound);

        StartCoroutine(RunAwayRoutine());

        yield return new WaitForSeconds(fireCooldown);
        canFire = true;
    }

    private IEnumerator RunAwayRoutine()
    {
        if (_impulseController.IsInImpulse())
            yield break;

        if (!FindBestRunawayPosition(out Vector3 runPosition))
            yield break;

        StartRunningAway(runPosition);
        yield return WaitForDestinationOrTimeout();
        StopRunningAway();
    }

    private void StartRunningAway(Vector3 destination)
    {
        isRunningAway = true;
        agent.isStopped = false;
        agent.speed = originalSpeed * _attributes.RunBackSpeedMultiplier;
        agent.SetDestination(destination);
    }

    private IEnumerator WaitForDestinationOrTimeout()
    {
        float startTime = Time.time;

        while (!_impulseController.IsInImpulse())
        {
            if (HasTimedOut(startTime))
                yield break;

            if (agent.pathPending)
            {
                yield return null;
                continue;
            }

            if (HasReachedDestination())
                yield break;

            yield return null;
        }
    }

    private bool HasTimedOut(float startTime) => Time.time - startTime >= runBackTimeout;

    private bool HasReachedDestination() => agent.remainingDistance <= agent.stoppingDistance;

    private void StopRunningAway()
    {
        if (!_impulseController.IsInImpulse())
            agent.speed = originalSpeed;

        isRunningAway = false;
    }

    private bool FindBestRunawayPosition(out Vector3 finalPosition)
    {
        Vector3 backwardDirection = -transform.up;

        var sortedOptions = Enumerable.Range(0, _attributes.RaycastCount)
            .Select(i => GetRunawayOption(backwardDirection, i))
            .OrderByDescending(option => option.Distance);

        foreach (var option in sortedOptions)
        {
            if (TryGetNavMeshPosition(option, out finalPosition))
                return true;
        }

        finalPosition = Vector3.zero;
        return false;
    }

    private bool TryGetNavMeshPosition((Vector3 Direction, float Distance) option, out Vector3 position)
    {
        Vector3 targetPos = transform.position + option.Direction * option.Distance;

        if (NavMesh.SamplePosition(targetPos, out NavMeshHit hit, 1f, NavMesh.AllAreas))
        {
            position = hit.position;
            return true;
        }

        position = Vector3.zero;
        return false;
    }

    private bool HasLineOfSight()
    {
        Vector2 originCenter = firePoint.transform.position;
        Vector2 direction = (player.position - firePoint.transform.position).normalized;
        float distance = Vector2.Distance(originCenter, player.position);

        Vector2 perp = new(-direction.y, direction.x);

        Vector2 originLeft = originCenter + perp * 0.35f;
        Vector2 originRight = originCenter - perp * 0.35f;

        LayerMask environmentMask = LayerMask.GetMask("Environment");

        bool centerBlocked = Physics2D.Raycast(originCenter, direction, distance, environmentMask);
        bool leftBlocked = Physics2D.Raycast(originLeft, direction, distance, environmentMask);
        bool rightBlocked = Physics2D.Raycast(originRight, direction, distance, environmentMask);

        if (centerBlocked || leftBlocked || rightBlocked)
            return false;

        return true;
    }

    private (Vector3 Direction, float Distance) GetRunawayOption(Vector3 baseDirection, int rayIndex)
    {
        float angle = GetAngleForRaycast(rayIndex);
        Vector3 direction = Quaternion.Euler(0, 0, angle) * baseDirection;
        float distance = GetAvailableDistance(direction);
        return (direction, distance);
    }

    private float GetAngleForRaycast(int rayIndex)
    {
        if (_attributes.RaycastCount == 1)
            return 0f;

        float step = _attributes.RaycastSpread / (_attributes.RaycastCount - 1);
        return -_attributes.RaycastSpread / 2f + step * rayIndex;
    }

    private float GetAvailableDistance(Vector3 direction)
    {
        RaycastHit2D hit = Physics2D.Raycast(
            transform.position,
            direction,
            _attributes.RunBackDistance,
            obstacleLayerMask
        );

        if (hit.collider != null)
            return hit.distance * _attributes.SafetyDistanceMultiplier;

        return _attributes.RunBackDistance;
    }

    private void PlayAttackAnimation()
    {
        if (_animator)
            _animator.TriggerAttack();
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
        if (!player) return;

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

    private void OnDrawGizmosSelected()
    {
        if (_attributes == null) return;

        Vector3 baseDirection = -transform.up;
        float maxDistance = 0f;
        Vector3 bestDirection = baseDirection;

        for (int i = 0; i < _attributes.RaycastCount; i++)
        {
            float angle = GetAngleForRaycast(i);
            Vector3 direction = Quaternion.Euler(0, 0, angle) * baseDirection;

            RaycastHit2D hit = Physics2D.Raycast(
                transform.position,
                direction,
                _attributes.RunBackDistance,
                obstacleLayerMask
            );

            float distance;
            Vector3 endPoint;

            if (hit.collider != null)
            {
                distance = hit.distance * _attributes.SafetyDistanceMultiplier;
                endPoint = transform.position + direction * distance;

                // Draw hit rays in yellow
                Gizmos.color = new Color(1f, 0.8f, 0f, 0.6f);
                Gizmos.DrawLine(transform.position, hit.point);

                // Draw obstacle hit point
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(hit.point, 0.2f);

                // Draw safe endpoint
                Gizmos.color = new Color(1f, 0.5f, 0f, 0.8f);
                Gizmos.DrawSphere(endPoint, 0.15f);
            }
            else
            {
                distance = _attributes.RunBackDistance;
                endPoint = transform.position + direction * distance;

                // Draw clear rays in green
                Gizmos.color = new Color(0f, 1f, 0f, 0.4f);
                Gizmos.DrawLine(transform.position, endPoint);

                // Draw endpoint
                Gizmos.color = new Color(0f, 1f, 0f, 0.6f);
                Gizmos.DrawSphere(endPoint, 0.15f);
            }

            // Track best option
            if (distance > maxDistance)
            {
                maxDistance = distance;
                bestDirection = direction;
            }
        }

        // Highlight the best direction
        Vector3 bestEndPoint = transform.position + bestDirection * maxDistance;
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(transform.position, bestEndPoint);
        Gizmos.DrawWireSphere(bestEndPoint, 0.3f);

        // Draw base backward direction for reference
        Gizmos.color = new Color(1f, 1f, 1f, 0.3f);
        Gizmos.DrawLine(transform.position, transform.position + baseDirection * 1f);
    }
}
