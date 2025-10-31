using UnityEngine;
using System.Collections;
using System;

public class PlayerMeleeController : MonoBehaviour
{
    [SerializeField] private float damageDelay = 0;
    [SerializeField] private string _layerName;
    [SerializeField] private float _animDuration;

    private bool _isAttacking = false;

    [SerializeField] private PlayerAnimator _animator;

    //private Collider2D testing;

    private Vector2 GetContactPoint(Collider2D otherCollider)
    {
        //testing = otherCollider;

        Collider2D myCollider = GetComponent<Collider2D>();
        return otherCollider.ClosestPoint(transform.TransformPoint(myCollider.offset));
    }

    // Contact point visualization
    /*void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        
        Vector2 closestPoint = testing.ClosestPoint(transform.TransformPoint(GetComponent<Collider2D>().offset));
        Gizmos.DrawSphere(new Vector3(closestPoint.x, closestPoint.y, 0f), 0.2f);
    }*/

    public void OnTriggerEnter2D(Collider2D otherCollider)
    {
        var health = otherCollider.GetComponent(typeof(IHealthController)) as IHealthController;
        
        if (
            IsEnemy(otherCollider) &&
            health != null &&
            !_isAttacking
        )
        {
            ShieldController enemyShield = otherCollider.gameObject.GetComponent<ShieldController>();
            if (
                enemyShield == null || 
                !enemyShield.ShieldBlocks(GetContactPoint(otherCollider))
            )
            {
                StartCoroutine(Attack(otherCollider.gameObject));
            }
            PlayAttackAnimation();
        }
    }

    private bool IsEnemy(Collider2D otherCollider) =>
        otherCollider.gameObject.layer == LayerMask.NameToLayer(_layerName);

    private IEnumerator Attack(GameObject enemyObject)
    {
        yield return new WaitForSeconds(damageDelay);

        if (enemyObject)
            enemyObject.GetComponent<IHealthController>().TakeDamage(PlayerStatsManager.Instance.MeleeDamage);
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
