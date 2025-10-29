using UnityEngine;
using System.Collections;

public class PirateMeleeController : MonoBehaviour
{
    [SerializeField] private PirateAttributes _pirateAttributes;
    [SerializeField] private MeleeEnemyAnimator _animator;

    [SerializeField] private float _damageDelay = 0;
    [SerializeField] private float _animDuration;

    private bool _isAttacking = false;

    public void OnTriggerEnter2D(Collider2D otherCollider)
    {
        var health = otherCollider.GetComponent(typeof(IHealthController)) as IHealthController;
        if (
            IsEnemy(otherCollider) &&
            health != null &&
            !_isAttacking
        )
        {
            StartCoroutine(Attack(otherCollider.gameObject));
            PlayAttackAnimation();
        }
    }

    private bool IsEnemy(Collider2D otherCollider) => 
        otherCollider.gameObject.layer == LayerMask.NameToLayer("Friendly");
    

    private IEnumerator Attack(GameObject enemyObject)
    {
        yield return new WaitForSeconds(_damageDelay);

        if (enemyObject)
            enemyObject.GetComponent<IHealthController>().TakeDamage(_pirateAttributes.DamageAmount);
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