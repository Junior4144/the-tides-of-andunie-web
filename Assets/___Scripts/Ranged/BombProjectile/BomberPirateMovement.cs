using System.Collections;
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
    public float runBackDistance = 5f;
    public float runDuration = 2f;
    public float runBackSpeedMultiplier = 2f;

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

    // Stuck detection
    private GameObject runawayTargetObject;
    private float stuckCheckTime = 0f;
    private float stuckThreshold = 0.2f;
    private float minVelocityThreshold = 0.05f;

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
        agent.speed = originalSpeed * runBackSpeedMultiplier;

        // Compute backward direction with random angle
        Vector3 backward = -transform.up;
        float randomAngle = Random.Range(-45f, 45f);
        backward = Quaternion.Euler(0, 0, randomAngle) * backward;
        Vector3 targetPos = transform.position + backward * runBackDistance;

        // Create temporary runaway target object
        if (runawayTargetObject != null)
            Destroy(runawayTargetObject);

        runawayTargetObject = new GameObject("BomberRunTarget");
        runawayTargetObject.transform.position = targetPos;

        // Move toward it
        agent.SetDestination(runawayTargetObject.transform.position);

        float elapsed = 0f;
        stuckCheckTime = 0f;

        while (elapsed < runDuration)
        {
            elapsed += Time.deltaTime;

            // STUCK DETECTION
            if (agent.velocity.magnitude < minVelocityThreshold)
            {
                stuckCheckTime += Time.deltaTime;

                if (stuckCheckTime > stuckThreshold)
                {
                    Debug.Log("Bomber stuck → aborting runaway.");
                    break;
                }
            }
            else
            {
                stuckCheckTime = 0f;
            }

            yield return null;
        }

        // Reset states
        agent.speed = originalSpeed;
        isRunningAway = false;

        if (runawayTargetObject != null)
            Destroy(runawayTargetObject);
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
