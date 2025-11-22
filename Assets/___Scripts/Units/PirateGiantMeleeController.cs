using UnityEngine;
using System.Collections;

public class PirateGiantMeleeController : MonoBehaviour
{
    [SerializeField] private PirateAttributes _pirateAttributes;
    [SerializeField] private GiantEnemyAnimator _animator;
    [SerializeField] private float _animDuration;

    [SerializeField] private float _damageDelay = 0f;
    [SerializeField] private float _damageRange = 2f;
    [SerializeField] private float _attackCooldown = 1.5f;

    [SerializeField] private CapsuleCollider2D _capsuleCollider;
    public AudioClip _audioClip;
    private Coroutine _attackRoutine;
    private Coroutine _impulse;
    private GameObject _currentTarget;
    private AudioSource _audioSource;



    private void Awake()
    {
        _capsuleCollider.enabled = false;
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D otherCollider)
    {
        if (IsValidTarget(otherCollider))
        {
            _currentTarget = otherCollider.gameObject;

            if (_attackRoutine == null)
                _attackRoutine = StartCoroutine(AttackLoop());

            _impulse = StartCoroutine(EnableColliderForSeconds(_capsuleCollider, .25f));
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
                StopCoroutine(_impulse);
                _attackRoutine = null;
                _capsuleCollider.enabled = false;
            }

            _currentTarget = null;
        }
    }

    private bool IsValidTarget(Collider2D c)
    {
        return (
            c.GetComponent<PlayerHealthController>() != null
        );
    }

    private IEnumerator AttackLoop()
    {
        while (_currentTarget != null)
        {
            PlayAttackAnimation();
            yield return new WaitForSeconds(_damageDelay);

            TryDealDamage(_currentTarget);
            _audioSource.PlayOneShot(_audioClip);
            yield return new WaitForSeconds(_attackCooldown);
            
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
            target.GetComponent<PlayerHealthController>()
                .TakeDamage(_pirateAttributes.DamageAmount, DamageType.Melee);
        }
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

    public IEnumerator EnableColliderForSeconds(Collider2D col, float seconds)
    {
        yield return new WaitForSeconds(_damageDelay);
        col.enabled = true;
        yield return new WaitForSeconds(seconds);
        col.enabled = false;
    }
}