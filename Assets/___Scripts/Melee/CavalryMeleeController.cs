using UnityEngine;
using System.Collections;

public class CavalryMeleeController : MonoBehaviour
{
    [SerializeField] private CavalryAttributes _attributes;
    [SerializeField] private CavalryAnimator _animator;

    [SerializeField] private float _damageDelay = 0f;
    [SerializeField] private float _animDuration;
    [SerializeField] private float _damageRange = 5f;

    private float _lastAttackTime;
    private bool _isAttacking = false;

    void Awake()
    {
        _lastAttackTime = -_attributes.AttackCoolDown;
    }

    public void OnTriggerEnter2D(Collider2D otherCollider)
    {
        var health = otherCollider.GetComponent(typeof(HealthController)) as HealthController;
        if (
            IsEnemy(otherCollider) &&
            health != null &&
            !_isAttacking &&
            Time.time - _lastAttackTime > _attributes.AttackCoolDown
        )
        {
            _lastAttackTime = Time.time;
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
        {
            float distance = Vector2.Distance(transform.position, enemyObject.transform.position);
            
            if (distance <= _damageRange)
                enemyObject.GetComponent<HealthController>().TakeDamage(_attributes.DamageAmount);
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