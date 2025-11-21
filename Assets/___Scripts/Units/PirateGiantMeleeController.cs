using UnityEngine;
using System.Collections;

public class PirateGiantMeleeController : MonoBehaviour
{
    [SerializeField] private PirateAttributes _pirateAttributes;
    [SerializeField] private GiantEnemyAnimator _animator;

    [SerializeField] private float _damageDelay = 0f;
    [SerializeField] private float _animDuration;
    [SerializeField] private float _damageRange = 2f;

    private bool _isAttacking = false;

    public void OnTriggerEnter2D(Collider2D otherCollider)
    {
        var health = otherCollider.GetComponent(typeof(HealthController)) as HealthController;
        if (
            IsFriendly(otherCollider) &&
            health != null &&
            !_isAttacking
        )
        {
            Debug.Log($"[PirateMeleeController] Attack initiated {otherCollider.name}");
            StartCoroutine(Attack(otherCollider.gameObject));
            PlayAttackAnimation();
        }
    }

    private bool IsFriendly(Collider2D otherCollider) =>
        otherCollider.gameObject.layer == LayerMask.NameToLayer("Friendly");


    private IEnumerator Attack(GameObject enemyObject)
    {
        yield return new WaitForSeconds(_damageDelay);

        if (enemyObject)
        {
            float distance = Vector2.Distance(transform.position, enemyObject.transform.position);

            if (distance <= _damageRange)
            {
                Debug.Log($"[PirateMeleeController] Dealing damage {_pirateAttributes.DamageAmount} to {enemyObject.name}");
                enemyObject.GetComponent<HealthController>().TakeDamage(_pirateAttributes.DamageAmount, DamageType.Melee);
            }
            else
            {
                Debug.Log($"[PirateMeleeController] Target out of range {distance}/{_damageRange}");
            }
        }
    }

    private void PlayAttackAnimation()
    {
        if (_animator)
        {
            _isAttacking = true;
            _animator.TriggerAttack();
            StartCoroutine(ResetAttackAnimation());
        }
        else
            Debug.LogWarning("Animator is Null. Playing no Animation");
    }

    private IEnumerator ResetAttackAnimation()
    {
        yield return new WaitForSeconds(_animDuration);
        _isAttacking = false;
    }
}