using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Audio;

public class RangedPirateMovement : MonoBehaviour
{

    [SerializeField] private float _attackAnimDuration = .9f;
    [SerializeField] private RangedPirateAttributes _attributes;
    [SerializeField] private GameObject ProjectilePrefab;
    [SerializeField] private GameObject firePoint;
    [SerializeField] private AudioClip fireShotSound;

    private Transform player;
    private NavMeshAgent agent;
    private Rigidbody2D _rigidbody;
    private RangedPirateAnimator _animator;
    private AudioSource _audioSource;
    private ImpulseController _impulseController;
    private bool canFire = true;
    private bool ifStillInRange = false;
    public float fireCooldown = 1f;
    public float HoldFireCooldown = 1f;

    public float impulseForce = 10f;
    public float impulseDuration = 0;
    

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponentInChildren<RangedPirateAnimator>();
        _audioSource = GetComponent<AudioSource>();
        _impulseController = GetComponentInChildren<ImpulseController>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    void Update()
    {
        if (!agent.enabled || !PlayerManager.Instance) return;

        player = PlayerManager.Instance.transform;
        RotateTowardsPlayer();
        
        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= _attributes.ReadyDistance && canFire)
        {
            if (!HasLineOfSight())
            {
                // Can't see player → DO NOT SHOOT
                ifStillInRange = false;
                agent.isStopped = false;
                agent.SetDestination(player.position);
                _animator.SetPlayerInRange(false);
                return;
            }

            // LOS is clear → proceed with firing sequences
            if (ifStillInRange)
            {
                StartCoroutine(initiateHoldFiringSequence());
                PlayHoldFireAnimation();
                return;
            }

            ifStillInRange = true;
            StartCoroutine(InitiateFiringSequence());
            PlayAttackAnimation();
            return;
        }

        if (distance > _attributes.ReadyDistance)
        {
            ifStillInRange = false;
            _animator.SetPlayerInRange(false); // <-- Turn off FireHold when player leaves range
            RotateTowardsMovementDirection();
            agent.SetDestination(player.position);
        }
    }

    private IEnumerator InitiateFiringSequence()
    {
        canFire = false;
        agent.isStopped = true;

        yield return new WaitForSeconds(0.67f);

        Instantiate(ProjectilePrefab, firePoint.transform.position, transform.rotation);
        ApplyImpulse();

        _audioSource.PlayOneShot(fireShotSound);
        yield return new WaitForSeconds(fireCooldown);
        if (agent.enabled && agent.isOnNavMesh)
            agent.isStopped = false;
        canFire = true;
    }

    private IEnumerator initiateHoldFiringSequence() //TODO USE VARIABLES INSTEAD OF REDOING CODE
    {
        canFire = false;
        agent.isStopped = true;

        yield return new WaitForSeconds(HoldFireCooldown);

        Instantiate(ProjectilePrefab, firePoint.transform.position, transform.rotation);
        ApplyImpulse();

        _audioSource.PlayOneShot(fireShotSound);
        yield return new WaitForSeconds(HoldFireCooldown);
        if (agent.enabled && agent.isOnNavMesh)
            agent.isStopped = false;
        canFire = true;
    }

    private void PlayAttackAnimation()
    {
        if (_animator)
        {
            _animator.TriggerAttack();
            StartCoroutine(ResetAttackAnimation());
        }
        else
            Debug.LogWarning("Animator is Null. Playing no Animation");
    }

    private IEnumerator ResetAttackAnimation()
    {
        yield return new WaitForSeconds(_attackAnimDuration);
    }

    private void PlayHoldFireAnimation()
    {
        if (_animator)
        {
            _animator.SetPlayerInRange(true);
        }
            
        else
            Debug.LogWarning("Animator is Null. Playing no Animation");
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

    private bool HasLineOfSight()
    {
        Vector2 originCenter = firePoint.transform.position;
        Vector2 direction = (player.position - firePoint.transform.position).normalized;
        float distance = Vector2.Distance(originCenter, player.position);

        Vector2 perp = new Vector2(-direction.y, direction.x);

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

    void ApplyImpulse()
    {
        var impulseSettings = new ImpulseSettings
        {
            Force = impulseForce,
            Duration = impulseDuration,
            PlaySound = true,
            SpawnParticles = true
        };

        _impulseController.InitiateSquadImpulse(transform.position, -_rigidbody.transform.up, impulseSettings);
    }

    private void OnDrawGizmos()
    {
        if (player == null || firePoint == null) return;

        Vector2 originCenter = firePoint.transform.position;
        Vector2 direction = (player.position - firePoint.transform.position).normalized;
        float distance = Vector2.Distance(originCenter, player.position);

        Vector2 perp = new Vector2(-direction.y, direction.x);
        Vector2 originLeft = originCenter + perp * 0.35f;
        Vector2 originRight = originCenter - perp * 0.35f;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(originCenter, originCenter + direction * distance);

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(originLeft, originLeft + direction * distance);
        Gizmos.DrawLine(originRight, originRight + direction * distance);
    }
}
