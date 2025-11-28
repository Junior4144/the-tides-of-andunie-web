using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Audio;

public class RangedGiantPirateMovement : MonoBehaviour
{
    [Header("Ranged Attack Settings")]
    [SerializeField] private float _rangedAttackAnimDuration = .9f;
    [SerializeField] private GameObject ProjectilePrefab;
    [SerializeField] private GameObject firePoint;
    [SerializeField] private AudioClip fireShotSound;
    [SerializeField] private float fireCooldown = 1f;
    [SerializeField] private float HoldFireCooldown = 1f;
    [SerializeField] private float impulseForce = 10f;
    [SerializeField] private float impulseDuration = 0;
    [SerializeField] private float minRangedDistance = 6f; // Won't shoot if player is closer than this

    [Header("Melee Attack Settings")]
    [SerializeField] private float _meleeAttackAnimDuration = 1.067f;
    [SerializeField] private float meleeAttackRange = 3f;
    
    [Header("References")]
    [SerializeField] private RangedGiantPirateAttributes _attributes;

    private Transform player;
    private NavMeshAgent agent;
    private Rigidbody2D _rigidbody;
    private GiantRangedPirateAnimator _animator;
    private AudioSource _audioSource;
    private ImpulseController _impulseController;
    private Collider2D _attackCollider;
    
    private bool canAttack = true;
    private bool ifStillInRangedRange = false;
    private bool isMeleeAttacking = false;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponentInChildren<GiantRangedPirateAnimator>();
        _audioSource = GetComponent<AudioSource>();
        _impulseController = GetComponentInChildren<ImpulseController>();
        _attackCollider = GetComponent<Collider2D>();
        
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        
        // Disable PirateMovementController to prevent conflicts
        var pirateMovement = GetComponent<PirateMovementController>();
        if (pirateMovement != null)
        {
            pirateMovement.enabled = false;
        }
        
        // Set agent speed from attributes
        agent.speed = _attributes.MovementSpeed;
        agent.acceleration = _attributes.MovementSpeed * 6f;
        if (_attributes.Acceleration > 5)
        {
            agent.acceleration = _attributes.Acceleration;
        }
    }

    void Update()
    {
        if (!agent.enabled || !PlayerManager.Instance) return;

        player = PlayerManager.Instance.transform;
        
        float distance = Vector2.Distance(transform.position, player.position);
        Debug.Log($"[RangedGiantPirateMovement] Distance to player: {distance:F2}, ReadyDistance: {_attributes.ReadyDistance}, MeleeRange: {meleeAttackRange}");

        // Determine which attack mode to use based on distance
        if (distance <= meleeAttackRange)
        {
            // Close range - use melee attack (handled by OnTriggerEnter2D)
            Debug.Log($"[RangedGiantPirateMovement] In MELEE range - waiting for trigger");
            RotateTowardsPlayer();
            agent.isStopped = true;
        }
        else if (distance >= minRangedDistance && distance <= _attributes.ReadyDistance && canAttack)
        {
            // Medium range - use ranged attack (only if outside minimum range)
            Debug.Log($"[RangedGiantPirateMovement] In RANGED range - checking LOS");
            RotateTowardsPlayer();
            
            if (!HasLineOfSight())
            {
                // Can't see player → move closer
                Debug.Log($"[RangedGiantPirateMovement] No line of sight - moving closer");
                ifStillInRangedRange = false;
                agent.isStopped = false;
                agent.SetDestination(player.position);
                return;
            }

            // LOS is clear → proceed with firing sequences
            if (ifStillInRangedRange)
            {
                Debug.Log($"[RangedGiantPirateMovement] Hold firing sequence");
                StartCoroutine(initiateHoldFiringSequence());
                PlayRangedHoldFireAnimation();
                return;
            }

            Debug.Log($"[RangedGiantPirateMovement] Starting initial firing sequence");
            ifStillInRangedRange = true;
            StartCoroutine(InitiateFiringSequence());
            PlayRangedAttackAnimation();
            return;
        }
        else if (distance > meleeAttackRange && distance < minRangedDistance)
        {
            // Too close for ranged, too far for melee - move to maintain distance
            Debug.Log($"[RangedGiantPirateMovement] In DEAD ZONE (between melee and min ranged distance) - moving away");
            ifStillInRangedRange = false;
            RotateTowardsPlayer();
            
            // Move away from player to get to minRangedDistance
            Vector3 awayDirection = (transform.position - player.position).normalized;
            Vector3 targetPosition = player.position + awayDirection * minRangedDistance;
            agent.isStopped = false;
            agent.SetDestination(targetPosition);
        }
        else
        {
            // Far away - move towards player
            Debug.Log($"[RangedGiantPirateMovement] TOO FAR - moving towards player");
            ifStillInRangedRange = false;
            RotateTowardsMovementDirection();
            agent.SetDestination(player.position);
        }
    }

    private void OnTriggerEnter2D(Collider2D otherCollider)
    {
        if (!otherCollider.CompareTag("Player") || isMeleeAttacking || !canAttack)
            return;

        StartCoroutine(PerformMeleeAttack());
    }

    // ===== MELEE ATTACK METHODS =====
    private IEnumerator PerformMeleeAttack()
    {
        isMeleeAttacking = true;
        canAttack = false;
        agent.isStopped = true;

        PlayMeleeAttackAnimation();

        yield return new WaitForSeconds(_attributes.DamageDelay);

        TryDealMeleeDamageAndImpulse();

        yield return new WaitForSeconds(_meleeAttackAnimDuration - _attributes.DamageDelay);
        
        isMeleeAttacking = false;
        canAttack = true;
        agent.isStopped = false;
    }

    private void TryDealMeleeDamageAndImpulse()
    {
        if (!PlayerManager.Instance)
            return;

        if (!IsPlayerInMeleeRange())
            return;

        DealMeleeDamageToPlayer();
        ApplyMeleeImpulseToPlayer();
    }

    private bool IsPlayerInMeleeRange()
    {
        if (!player) return false;
        
        float distance = Vector2.Distance(transform.position, player.position);
        return distance <= _attributes.DamageRange;
    }

    private void DealMeleeDamageToPlayer()
    {
        PlayerManager.Instance.TakeDamage(_attributes.DamageAmount, DamageType.Melee);
    }

    private void ApplyMeleeImpulseToPlayer()
    {
        Vector2 contactPoint = CalculateMeleeContactPoint();
        Vector2 impulseDirection = CalculateMeleeImpulseDirection();
        ImpulseSettings settings = CreateMeleeImpulseSettings();

        PlayerManager.Instance.ApplyImpulse(contactPoint, impulseDirection, settings);
    }

    private Vector2 CalculateMeleeContactPoint()
    {
        Vector2 playerPosition = PlayerManager.Instance.GetPlayerTransform().position;
        return _attackCollider.ClosestPoint(playerPosition);
    }

    private Vector2 CalculateMeleeImpulseDirection()
    {
        Vector2 playerPosition = PlayerManager.Instance.GetPlayerTransform().position;
        return (playerPosition - (Vector2)transform.position).normalized;
    }

    private ImpulseSettings CreateMeleeImpulseSettings()
    {
        return new ImpulseSettings
        {
            Force = _attributes.ImpulseForce,
            Duration = _attributes.ImpulseDuration,
            PlaySound = true,
            SpawnParticles = true
        };
    }

    private void PlayMeleeAttackAnimation()
    {
        if (_animator)
        {
            _animator.TriggerMeleeAttack();
        }
        else
            Debug.LogWarning("[RangedGiantPirateMovement] Melee Animator is Null. Playing no Animation");
    }

    // ===== RANGED ATTACK METHODS =====
    private IEnumerator InitiateFiringSequence()
    {
        Debug.Log($"[RangedGiantPirateMovement] InitiateFiringSequence started");
        canAttack = false;
        agent.isStopped = true;

        yield return new WaitForSeconds(0.67f);

        Debug.Log($"[RangedGiantPirateMovement] Firing cannon projectile!");
        Instantiate(ProjectilePrefab, firePoint.transform.position, transform.rotation);
        ApplyCannonImpulse();

        if (fireShotSound)
            _audioSource.PlayOneShot(fireShotSound);
        
        yield return new WaitForSeconds(fireCooldown);
        
        if (agent.enabled && agent.isOnNavMesh)
            agent.isStopped = false;
        
        canAttack = true;
    }

    private IEnumerator initiateHoldFiringSequence()
    {
        canAttack = false;
        agent.isStopped = true;

        yield return new WaitForSeconds(HoldFireCooldown);

        Instantiate(ProjectilePrefab, firePoint.transform.position, transform.rotation);
        ApplyCannonImpulse();

        if (fireShotSound)
            _audioSource.PlayOneShot(fireShotSound);
        
        yield return new WaitForSeconds(HoldFireCooldown);
        
        if (agent.enabled && agent.isOnNavMesh)
            agent.isStopped = false;
        
        canAttack = true;
    }

    private void PlayRangedAttackAnimation()
    {
        Debug.Log($"[RangedGiantPirateMovement] PlayRangedAttackAnimation called");
        if (_animator)
        {
            Debug.Log($"[RangedGiantPirateMovement] Triggering CANNON attack animation");
            _animator.TriggerCannonAttack();
            StartCoroutine(ResetRangedAttackAnimation());
        }
        else
            Debug.LogWarning("[RangedGiantPirateMovement] Animator is Null. Playing no Animation");
    }

    private IEnumerator ResetRangedAttackAnimation()
    {
        yield return new WaitForSeconds(_rangedAttackAnimDuration);
    }

    private void PlayRangedHoldFireAnimation()
    {
        // Not used with GiantEnemyAnimator - it doesn't have a hold fire state
        // Just trigger the cannon attack directly
        if (_animator)
        {
            _animator.TriggerCannonAttack();
        }
    }

    // ===== ROTATION METHODS =====
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

    // ===== LINE OF SIGHT =====
    private bool HasLineOfSight()
    {
        if (!player || !firePoint) return false;
        
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

    // ===== CANNON IMPULSE =====
    void ApplyCannonImpulse()
    {
        if (!_impulseController) return;
        
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
