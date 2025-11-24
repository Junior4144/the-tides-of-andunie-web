using UnityEngine;
using System.Collections;

public class PirateGiantMeleeController : MonoBehaviour
{
    [SerializeField] private PirateAttributes _pirateAttributes;
    [SerializeField] private GiantEnemyAnimator _animator;
    [SerializeField] private float _animDuration;

    [SerializeField] private float _damageDelay = 0f;
    [SerializeField] private float _damageRange = 2f;

    [SerializeField] private CapsuleCollider2D _impulseCollider;
    [SerializeField] private float _impulseColliderDuration = 0.1f;

    private bool isAttacking;

    private void Awake()
    {
        _impulseCollider.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D otherCollider)
    {
        if (!otherCollider.CompareTag("Player") || isAttacking)
            return;

        StartCoroutine(PerformAttack());
    }

    private IEnumerator PerformAttack()
    {
        isAttacking = true;

        PlayAttackAnimation();

        yield return new WaitForSeconds(_damageDelay);

        TryDealDamageToPlayer();
        StartCoroutine(EnableColliderForSeconds(_impulseCollider, _impulseColliderDuration));

        isAttacking = false;
    }

    private void TryDealDamageToPlayer()
    {
        if (!PlayerManager.Instance)
            return;

        float distance = Vector2.Distance(transform.position, PlayerManager.Instance.GetPlayerTransform().position);

        if (distance <= _damageRange)
             PlayerManager.Instance.TakeDamage(_pirateAttributes.DamageAmount, DamageType.Melee);
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

    public IEnumerator EnableColliderForSeconds(Collider2D collider, float seconds)
    {
        collider.enabled = true;
        yield return new WaitForSeconds(seconds);
        collider.enabled = false;
    }
}