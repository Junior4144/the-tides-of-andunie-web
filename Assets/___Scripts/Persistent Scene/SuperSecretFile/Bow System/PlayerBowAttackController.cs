using System.Collections;
using UnityEngine;


public class PlayerBowAttackController : MonoBehaviour
{
    [SerializeField] private Transform playerRoot;
    [SerializeField] private float damageDelay = 0;
    [SerializeField] private string _layerName;
    [SerializeField] private float _animDuration;
    [SerializeField] private AudioClip _attackSound;
    [SerializeField] private float attackTurnSpeed = 15f;
    [SerializeField] private PlayerAnimator _animator;
    [SerializeField] private float rotationThreshold = 1f; // how close to target angle before stopping

    private bool _isAttacking = false;
    private float targetAngle;
    private bool _isRotating = false;
    private AudioSource _audioSource;
    private bool _queuedAttack = false;

    public bool IsAttacking => _isAttacking;
    public float AttackDuration => _animDuration;

    public BowWeapon bowWeapon;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        // When player clicks
        if (Input.GetMouseButtonDown(0))
        {
            if (!_isAttacking && !_isRotating)
            {
                // Start attack normally
                SetTargetDirection();
                StartRotateToTarget();
            }
            else if (_isAttacking && !_queuedAttack)
            {
                // Queue one attack only
                _queuedAttack = true;
            }
        }
    }

    private void SetTargetDirection()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePos - playerRoot.position);
        targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
    }

    private void StartRotateToTarget()
    {
        _isAttacking = true;
        if (!_isRotating)
            StartCoroutine(RotateToTargetCoroutine());
    }

    private IEnumerator RotateToTargetCoroutine()
    {
        _isRotating = true;

        while (true)
        {
            Quaternion targetRot = Quaternion.Euler(0, 0, targetAngle);
            playerRoot.rotation = Quaternion.RotateTowards(
                playerRoot.rotation,
                targetRot,
                attackTurnSpeed * Time.deltaTime
            );

            float angleDiff = Quaternion.Angle(playerRoot.rotation, targetRot);
            if (angleDiff < rotationThreshold)
                break;

            yield return null;
        }

        _isRotating = false;
        OnRotationComplete();
    }

    private void OnRotationComplete()
    {
        PlayAttackAnimation();
    }

    private void PlayAttackAnimation()
    {
        //if (_animator)
        //    _animator.TriggerAttack();

            bowWeapon.Fire();

        if (_attackSound)
            _audioSource.PlayOneShot(_attackSound, 0.4f);

        StartCoroutine(ResetAttackAnimation());
    }

    private IEnumerator ResetAttackAnimation()
    {
        yield return new WaitForSeconds(_animDuration);
        _isAttacking = false;

        // If queued attack exists, immediately trigger it
        if (_queuedAttack)
        {
            _queuedAttack = false;
            SetTargetDirection();
            StartRotateToTarget();
        }
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
        }
        else if (!_queuedAttack)
        {
            _queuedAttack = true;
        }
    }
}
