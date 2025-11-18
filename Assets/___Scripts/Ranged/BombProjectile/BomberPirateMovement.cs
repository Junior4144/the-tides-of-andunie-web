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

    private Transform player;
    private NavMeshAgent agent;
    private Rigidbody2D _rigidbody;
    private ArcherPirateAnimator _animator;
    private AudioSource _audioSource;
    private ImpulseController _impulseController;
    private bool canFire = true;
    public float fireCooldown = 1f;
    public float HoldFireCooldown = 1f;

    public float impulseForce = 10f;
    public float impulseDuration = 0;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponentInChildren<ArcherPirateAnimator>();
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
            //ifStillInRange = true;
            StartCoroutine(FireSequence());
            PlayAttackAnimation();
            return;
        }

        if (distance > _attributes.ReadyDistance)
        {
            //ifStillInRange = false;
            RotateTowardsMovementDirection();
            agent.SetDestination(player.position);
        }
    }

    private IEnumerator FireSequence()
    {
        canFire = false;
        agent.isStopped = true;

        // Small delay for aim/animation sync
        yield return new WaitForSeconds(_attackAnimDuration);

        Instantiate(ProjectilePrefab, firePoint.transform.position, transform.rotation);
        ApplyImpulse();

        if (fireShotSound)
            _audioSource.PlayOneShot(fireShotSound);

        yield return new WaitForSeconds(fireCooldown);

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
        {
            Debug.LogWarning("Animator is Null. Playing no Animation");
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

    void ApplyImpulse()
    {
        var impulseSettings = new ImpulseSettings
        {
            Force = impulseForce,
            Duration = impulseDuration,
            PlaySound = true,
            SpawnParticles = true
        };

        //_impulseController.InitiateSquadImpulse(transform.position, -_rigidbody.transform.up, impulseSettings);
    }
}