using UnityEngine;
using System.Collections;
using System;

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

    [SerializeField]
    private PlayerAnimator _animator;


    public static event Action<float> OnDamageChanged;

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
    otherCollider.gameObject.layer == LayerMask.NameToLayer(_layerName);

    private IEnumerator Attack(GameObject enemyObject)
    {
        yield return new WaitForSeconds(damageDelay);

        if (enemyObject)
            enemyObject.GetComponent<IHealthController>().TakeDamage(_damage);
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
        {
            Debug.LogWarning("Animator is Null. Playing no Animation");
        }
    }

    private IEnumerator ResetAttackAnimation()
    {
        yield return new WaitForSeconds(_animDuration);
        _isAttacking = false;
    }

    public void AddDamage(float damageToAdd)
    {
        _damage += damageToAdd;
        OnDamageChanged?.Invoke(_damage);
    }

    public float GetDamageAmount() => _damage;
    public void SetDamageAmount(float currentDamage) => _damage = currentDamage;

}
