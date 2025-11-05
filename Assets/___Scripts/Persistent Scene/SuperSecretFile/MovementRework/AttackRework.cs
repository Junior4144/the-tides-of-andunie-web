using System.Collections;
using UnityEngine;
using UnityEngine.LowLevel;
using UnityEngine.Rendering;

[RequireComponent(typeof(AudioSource))]
public class AttackRework : MonoBehaviour
{
    [SerializeField] private Transform playerRoot; // assign parent in inspector

    [SerializeField] private float damageDelay = 0;
    [SerializeField] private string _layerName;
    [SerializeField] private float _animDuration;
    [SerializeField] private AudioClip _attackSound;

    [SerializeField] private float attackTurnSpeed = 15f;

    private bool _isAttacking = false;
    public float AttackDuration => _animDuration;
    [SerializeField] private PlayerAnimator _animator;
    private AudioSource _audioSource;

    public bool IsAttacking => _isAttacking;

    private float targetAngle;


    [Header("Hop Dash Settings")]
    [SerializeField] private float hopDistance = 0.20f; // tiny dash forward


    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !_isAttacking)
        {
            SetTargetDirection();
            HopThenAttack();
        }

        if (_isAttacking)
        {
            SmoothRotateToTarget();
        }
    }

    private void SetTargetDirection()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePos - playerRoot.position);
        targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f; // face up corrected
    }

    private void SmoothRotateToTarget()
    {
        Quaternion targetRot = Quaternion.Euler(0, 0, targetAngle);
        playerRoot.rotation = Quaternion.Lerp(playerRoot.rotation, targetRot, attackTurnSpeed * Time.deltaTime);
    }

    private void HopThenAttack()
    {
        _isAttacking = true;

        // Face the target direction first
        playerRoot.rotation = Quaternion.Euler(0, 0, targetAngle);

        // Small step forward (sprite faces up, so use .up)
        playerRoot.position += playerRoot.up * hopDistance;

        // now attack
        PlayAttackAnimation();
    }

    private void PlayAttackAnimation()
    {
        if (_animator)
            _animator.TriggerAttack();

        if (_attackSound)
            _audioSource.PlayOneShot(_attackSound, 0.4f);

        StartCoroutine(ResetAttackAnimation());
    }

    private IEnumerator ResetAttackAnimation()
    {
        yield return new WaitForSeconds(_animDuration);
        _isAttacking = false;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!_isAttacking) return;
        if (col.gameObject.layer != LayerMask.NameToLayer(_layerName)) return;

        if (col.TryGetComponent(out IHealthController health))
            StartCoroutine(DealDamage(health));
    }

    private IEnumerator DealDamage(IHealthController health)
    {
        yield return new WaitForSeconds(damageDelay);
        health.TakeDamage(PlayerStatsManager.Instance.MeleeDamage);
    }

    public void TriggerAttackExternal()
    {
        if (!_isAttacking)
        {
            SetTargetDirection();
            HopThenAttack();
        }
    }
}