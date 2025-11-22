using UnityEngine;
using System.Collections;

public class PirateGiantMeleeController : MonoBehaviour
{
    [SerializeField] private PirateAttributes _pirateAttributes;
    [SerializeField] private GiantEnemyAnimator _animator;
    [SerializeField] private float _animDuration;

    [SerializeField] private float _damageDelay = 0f;
    [SerializeField] private float _damageRange = 2f;
    [SerializeField] private float _attackCooldown = 1.5f;   // Add this

    private Coroutine _attackRoutine;
    private GameObject _currentTarget;

    private CapsuleCollider2D _capsuleCollider;
    private void Awake()
    {
        _capsuleCollider = GetComponentInChildren<CapsuleCollider2D>();
        _capsuleCollider.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D otherCollider)
    {
        if (IsValidTarget(otherCollider))
        {
            _currentTarget = otherCollider.gameObject;

            if (_attackRoutine == null)
                _attackRoutine = StartCoroutine(AttackLoop());
        }
    }

    private void OnTriggerExit2D(Collider2D otherCollider)
    {
        if (_currentTarget == otherCollider.gameObject)
        {
            // Stop attacking when target leaves
            if (_attackRoutine != null)
            {
                StopCoroutine(_attackRoutine);
                _attackRoutine = null;
            }

            _currentTarget = null;
        }
    }

    private bool IsValidTarget(Collider2D c)
    {
        return (
            c.gameObject.layer == LayerMask.NameToLayer("Friendly") &&
            c.GetComponent<HealthController>() != null
        );
    }

    private IEnumerator AttackLoop()
    {
        while (_currentTarget != null)
        {
            PlayAttackAnimation();
            yield return new WaitForSeconds(_damageDelay);
            _capsuleCollider.enabled = true;
            TryDealDamage(_currentTarget);

            yield return new WaitForSeconds(_attackCooldown);
            _capsuleCollider.enabled = true;
        }

        _attackRoutine = null;
    }

    private void TryDealDamage(GameObject target)
    {
        if (!target)
            return;

        float dist = Vector2.Distance(transform.position, target.transform.position);

        if (dist <= _damageRange)
        {
            target.GetComponent<HealthController>()
                .TakeDamage(_pirateAttributes.DamageAmount, DamageType.Melee);
        }
    }

    private void PlayAttackAnimation()
    {
        if (_animator)
            _animator.TriggerAttack();
    }
}