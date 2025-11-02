using UnityEngine;
using System.Collections;
using UnityEngine.Rendering;

[RequireComponent(typeof(AudioSource))]
public class PlayerMeleeController : MonoBehaviour
{
    [SerializeField] private float damageDelay = 0;
    [SerializeField] private string _layerName;
    [SerializeField] private float _animDuration;
    [SerializeField] private AudioClip _attackSound;

    private bool _isAttacking = false;

    [SerializeField] private PlayerAnimator _animator;
    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private Vector2 GetContactPoint(Collider2D otherCollider)
    {
        Collider2D myCollider = GetComponent<Collider2D>();
        return otherCollider.ClosestPoint(transform.TransformPoint(myCollider.offset));
    }

    public void OnTriggerEnter2D(Collider2D otherCollider)
    {
        if (
            IsEnemy(otherCollider) &&
            otherCollider.GetComponent(typeof(IHealthController)) is IHealthController &&
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
            PlayAttackSound();
            StartCoroutine(ResetAttackAnimation());
        }
        else
            Debug.LogWarning("Animator is Null. Playing no Animation");
    }

    private void PlayAttackSound()
    {
        if (_attackSound != null)
            _audioSource.PlayOneShot(_attackSound, volumeScale: 0.4f);
        else
            Debug.LogWarning("[PlayerMeleeController] Attack sound is null. Playing no sound");
    }

    private IEnumerator ResetAttackAnimation()
    {
        yield return new WaitForSeconds(_animDuration);
        _isAttacking = false;
    }
}
