using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class RangedGiantPirateMovement : MonoBehaviour
{

    [Header("References")]
    [SerializeField] private RangedGiantAttributes _attributes;
    [SerializeField] private GameObject ProjectilePrefab;
    [SerializeField] private GameObject firePoint;
    [SerializeField] private AudioClip fireShotSound;
    [SerializeField] private Collider2D _meleeAttackCollider;

    private Transform player;
    private NavMeshAgent agent;
    private Rigidbody2D _rigidbody;
    private GiantRangedPirateAnimator _animator;
    private AudioSource _audioSource;
    private ImpulseController _impulseController;
    private Collider2D _attackCollider;
    
    private enum CombatState
    {
        Default,
        Ranged,
        Melee
    }

    private CombatState _currentState = CombatState.Default;
    private bool _canAttack = true;
    private bool _isMeleeAttacking = false;

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

        // Disable melee attack collider by default (enabled only in Melee state)
        if (_meleeAttackCollider != null)
        {
            _meleeAttackCollider.enabled = false;
        }
    }

    // ===== STATE MACHINE =====
    void Update()
    {
        if (!agent.enabled || !PlayerManager.Instance) return;

        player = PlayerManager.Instance.transform;
        float distance = Vector2.Distance(transform.position, player.position);

        // Determine and transition to new state
        CombatState newState = DetermineState(distance);
        if (newState != _currentState)
        {
            OnStateChanged(_currentState, newState);
            _currentState = newState;
        }

        // Execute current state behavior
        switch (_currentState)
        {
            case CombatState.Default:
                HandleApproachingState();
                break;
            case CombatState.Ranged:
                HandleRangedState();
                break;
            case CombatState.Melee:
                HandleMeleeState();
                break;
        }
    }

    private CombatState DetermineState(float distanceToPlayer)
    {
        // Don't interrupt firing sequence - stay in Ranged state until complete
        if (!_canAttack && _currentState == CombatState.Ranged)
        {
            Debug.Log($"[RangedGiantPirateMovement] Firing in progress, staying in Ranged state");
            return CombatState.Ranged;
        }

        // Normal state determination based on distance
        if (distanceToPlayer < _attributes.RangedMeleeThreshold)
            return CombatState.Melee;

        if (distanceToPlayer <= _attributes.ReadyDistance)
            return CombatState.Ranged;

        return CombatState.Default;
    }

    private void OnStateChanged(CombatState fromState, CombatState toState)
    {
        Debug.Log($"[RangedGiantPirateMovement] State changed: {fromState} → {toState}");

        // Enable melee attack collider only in Melee state
        if (_meleeAttackCollider != null)
        {
            _meleeAttackCollider.enabled = (toState == CombatState.Melee);
        }
    }

    private void HandleApproachingState()
    {
        Debug.Log($"[RangedGiantPirateMovement] APPROACHING - moving towards player");
        RotateTowardsMovementDirection();
        agent.isStopped = false;
        agent.SetDestination(player.position);
    }

    private void HandleRangedState()
    {
        RotateTowardsPlayer();

        // Check line of sight - if blocked, move closer
        if (!HasLineOfSight())
        {
            Debug.Log($"[RangedGiantPirateMovement] RANGED - No line of sight, moving closer");
            agent.isStopped = false;
            agent.SetDestination(player.position);
            return;
        }

        // Line of sight clear - fire if ready
        if (_canAttack)
        {
            Debug.Log($"[RangedGiantPirateMovement] RANGED - Firing cannon");
            StartCoroutine(InitiateFiringSequence());
        }
        else
        {
            // Currently in firing sequence or cooldown
            agent.isStopped = true;
        }
    }

    private void HandleMeleeState()
    {
        Debug.Log($"[RangedGiantPirateMovement] MELEE - closing distance and waiting for trigger");
        RotateTowardsPlayer();

        // Move towards player until trigger collision occurs
        float distance = Vector2.Distance(transform.position, player.position);
        if (distance > _attributes.MeleeAttackRange)
        {
            agent.isStopped = false;
            agent.SetDestination(player.position);
        }
        else
        {
            agent.isStopped = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D otherCollider)
    {
        if (!otherCollider.CompareTag("Player") || _isMeleeAttacking || !_canAttack)
            return;

        StartCoroutine(PerformMeleeAttack());
    }

    // ===== MELEE ATTACK METHODS =====
    private IEnumerator PerformMeleeAttack()
    {
        _isMeleeAttacking = true;
        _canAttack = false;
        agent.isStopped = true;

        PlayMeleeAttackAnimation();

        yield return new WaitForSeconds(_attributes.DamageDelay);

        TryDealMeleeDamageAndImpulse();

        yield return new WaitForSeconds(_attributes.MeleeAttackAnimDuration - _attributes.DamageDelay);

        _isMeleeAttacking = false;
        _canAttack = true;
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
        Debug.Log($"[RangedGiantPirateMovement] Firing sequence started");
        _canAttack = false;
        agent.isStopped = true;

        // 1. Play cannon attack animation
        PlayRangedAttackAnimation();

        // 2. Wait for projectile spawn timing in animation
        yield return new WaitForSeconds(_attributes.ProjectileSpawnDelay);

        // 3. Spawn projectile and apply recoil
        Debug.Log($"[RangedGiantPirateMovement] Firing cannon projectile!");
        Instantiate(ProjectilePrefab, firePoint.transform.position, transform.rotation);
        ApplyCannonImpulse();

        if (fireShotSound)
            _audioSource.PlayOneShot(fireShotSound);

        // 4. Wait for rest of animation to complete
        float remainingAnimTime = _attributes.RangedAttackAnimDuration - _attributes.ProjectileSpawnDelay;
        yield return new WaitForSeconds(remainingAnimTime);

        // 5. Cooldown before next shot can be fired
        yield return new WaitForSeconds(_attributes.FireCooldown);

        // 6. Ready to fire again
        if (agent.enabled && agent.isOnNavMesh)
            agent.isStopped = false;

        _canAttack = true;
        Debug.Log($"[RangedGiantPirateMovement] Firing sequence complete, ready to fire again");
    }

    private void PlayRangedAttackAnimation()
    {
        if (_animator)
        {
            _animator.TriggerCannonAttack();
        }
        else
        {
            Debug.LogWarning("[RangedGiantPirateMovement] Animator is null, cannot play cannon attack animation");
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
            Force = _attributes.CannonImpulseForce,
            Duration = _attributes.CannonImpulseDuration,
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
