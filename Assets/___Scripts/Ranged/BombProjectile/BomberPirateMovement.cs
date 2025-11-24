using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class BomberPirateMovement : MonoBehaviour
{
    [SerializeField] private float _attackAnimDuration = .9f;
    [SerializeField] private PirateAttributes _attributes;
    [SerializeField] private GameObject ProjectilePrefab;
    [SerializeField] private GameObject firePoint;
    [SerializeField] private AudioClip fireShotSound;

    [Header("Runaway Settings")]
    [SerializeField] private LayerMask obstacleLayerMask;

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

        if (distance <= _attributes.ReadyDistance && canFire)
        {
            StartCoroutine(FireSequence());
            PlayAttackAnimation();
            return;
        }

        if (distance > _attributes.ReadyDistance)
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
        isRunningAway = true;
        agent.isStopped = false;
        agent.speed = originalSpeed * _attributes.RunBackSpeedMultiplier;

        agent.SetDestination(FindBestRunawayPosition());

        while (agent.pathPending || agent.remainingDistance > agent.stoppingDistance)
            yield return null;

        agent.speed = originalSpeed;
        isRunningAway = false;
    }

    private Vector3 FindBestRunawayPosition()
    {
        Vector3 baseDirection = -transform.up;

        var (direction, distance) = Enumerable.Range(0, _attributes.RaycastCount)
            .Select(i => GetRunawayOption(baseDirection, i))
            .OrderByDescending(option => option.Distance)
            .First();

        return transform.position + direction * distance;
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
        {
            _animator.TriggerAttack();
            StartCoroutine(ResetAttackAnimation());
        }
    }

    private IEnumerator ResetAttackAnimation()
    {
        yield return new WaitForSeconds(_attackAnimDuration);
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
}
