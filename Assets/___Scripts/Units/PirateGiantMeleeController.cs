using UnityEngine;
using System.Collections;

public class PirateGiantMeleeController : MonoBehaviour
{
    [SerializeField] private GiantPirateAttributes _attributes;
    [SerializeField] private GiantEnemyAnimator _animator;
    [SerializeField] private float _animDuration;

    private bool isAttacking;
    private Collider2D _attackCollider;

    private void Awake()
    {
        _attackCollider = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D otherCollider)
    {
        if (!otherCollider.gameObject.CompareTag("Player") || isAttacking)
            return;
        
        StartCoroutine(PerformAttack());
    }

    private IEnumerator PerformAttack()
    {
        isAttacking = true;

        PlayAttackAnimation();

        yield return new WaitForSeconds(_attributes.DamageDelay);

        TryDealDamageAndImpulse();

        isAttacking = false;
    }

    private void TryDealDamageAndImpulse()
    {
        if (!PlayerManager.Instance)
            return;

        if (!IsPlayerInRange())
            return;

        DealDamageToPlayer();
        ApplyImpulseToPlayer();
    }

    private bool IsPlayerInRange()
    {
        Vector2 playerPosition = PlayerManager.Instance.GetPlayerTransform().position;
        float distance = Vector2.Distance(transform.position, playerPosition);
        return distance <= _attributes.DamageRange;
    }

    private void DealDamageToPlayer() =>
        PlayerManager.Instance.TakeDamage(_attributes.DamageAmount, DamageType.Melee);

    private void ApplyImpulseToPlayer()
    {
        Vector2 contactPoint = CalculateContactPoint();
        Vector2 impulseDirection = CalculateImpulseDirection();
        ImpulseSettings settings = CreateImpulseSettings();

        PlayerManager.Instance.ApplyImpulse(contactPoint, impulseDirection, settings);
    }

    private Vector2 CalculateContactPoint()
    {
        Vector2 playerPosition = PlayerManager.Instance.GetPlayerTransform().position;

        return _attackCollider.ClosestPoint(playerPosition);
    }

    private Vector2 CalculateImpulseDirection()
    {
        Vector2 playerPosition = PlayerManager.Instance.GetPlayerTransform().position;

        return (playerPosition - (Vector2)transform.position).normalized;
    }

    private ImpulseSettings CreateImpulseSettings()
    {
        return new ImpulseSettings
        {
            Force = _attributes.ImpulseForce,
            Duration = _attributes.ImpulseDuration,
            PlaySound = true,
            SpawnParticles = true
        };
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
        yield return new WaitForSeconds(_animDuration);
    }
}