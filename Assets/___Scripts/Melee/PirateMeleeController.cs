using UnityEngine;
using System.Collections;

public class PirateMeleeController : MonoBehaviour
{
    [SerializeField]
    private PirateAttributes pirateAttributes;

    [SerializeField]
    private float damageDelay = 0;

    [SerializeField]
    private float _animDuration;

    private bool _isAttacking = false;

    public Animator animator;


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
        otherCollider.CompareTag("Player");

    private IEnumerator Attack(GameObject enemyObject)
    {
        yield return new WaitForSeconds(damageDelay);

        if (enemyObject)
            enemyObject.GetComponent<IHealthController>().TakeDamage(pirateAttributes.DamageAmount);
    }

    private void PlayAttackAnimation()
    {
        if (animator)
        {   
            _isAttacking = true;
            animator.SetBool("IsAttacking", true);
            StartCoroutine(ResetAttackAnimation());
        }
        else
        {
            Debug.LogWarning("Animator is Null. Playing no Animation");
        }
    }

    private IEnumerator ResetAttackAnimation()
    {
        yield return new WaitForSeconds(_animDuration);
        _isAttacking = false;
        animator.SetBool("IsAttacking", false);
    }
}