using UnityEngine;
using System.Collections;

public class MeleeController : MonoBehaviour
{
    [SerializeField]
    private float _damage = 20;

    [SerializeField]
    private float damageDelay = 0;

    [SerializeField]
    private string _layerName;

    [SerializeField]
    private float _animDuration;

    private bool _isAttacking = false;

    public Animator animator;


    public void OnTriggerEnter2D(Collider2D otherCollider)
    {
        if (
            IsEnemy(otherCollider) &&
            otherCollider.GetComponent<HealthController>() &&
            !_isAttacking
        )
        {
            StartCoroutine(Attack(otherCollider.gameObject));
            PlayAttackAnimation();
        }
    }

    private bool IsEnemy(Collider2D otherCollider) =>
    otherCollider.gameObject.layer == LayerMask.NameToLayer(_layerName);

    private IEnumerator Attack(GameObject enemyObject)
    {
        yield return new WaitForSeconds(damageDelay);
        enemyObject.GetComponent<HealthController>().TakeDamage(_damage);
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
