using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(CinemachineImpulseSource))]
public abstract class BaseAttack : MonoBehaviour
{
    [Header("References")]
    [SerializeField] protected PlayerAnimator _animator;
    [SerializeField] protected AudioClip _attackSound;
    [SerializeField] protected GameObject _hitEffectPrefab;
    [SerializeField] protected GameObject _impulseCollider;

    [Header("Settings")]
    [SerializeField] protected float _attackDuration = 0.5f;
    [SerializeField] protected float _damageDelay = 0f;

    [Header("Screen Shake Settings")]
    [SerializeField] protected float shakeCooldown = 0.2f;
    [SerializeField] protected float shakeForce = 1f;

    protected AudioSource _audioSrc;
    protected Rigidbody2D _rb;
    protected CinemachineImpulseSource _cameraImpulseSource;
    protected readonly HashSet<Collider2D> _hitEnemies = new();
    protected bool _isAttacking;
    protected bool _isShaking;

    public bool IsAttacking => _isAttacking;

    protected virtual void Awake()
    {
        _audioSrc = GetComponent<AudioSource>();
        _rb = GetComponentInParent<Rigidbody2D>();
        _cameraImpulseSource = GetComponent<CinemachineImpulseSource>();
    }

    protected virtual void OnEnable()
    {
        _animator.ReturnToDefaultIdle();
    }

    protected virtual void OnDisable()
    {
        StopAllCoroutines();
        _isAttacking = false;
        _isShaking = false;
        _hitEnemies.Clear();

        if (_impulseCollider != null)
            _impulseCollider.SetActive(false);

        transform.localRotation = Quaternion.identity;
    }

    protected virtual void Update()
    {
        if (_impulseCollider != null)
            _impulseCollider.SetActive(_isAttacking);
    }

    public abstract void Execute();

    protected abstract void OnTriggerEnter2D(Collider2D col);

    protected virtual IEnumerator DealDamageRoutine(HealthController health)
    {
        yield return new WaitForSeconds(_damageDelay);
        health.TakeDamage(PlayerStatsManager.Instance.MeleeDamage);
    }

    protected void SpawnHitEffect(Vector2 enemyPos)
    {
        if (_hitEffectPrefab == null) return;

        Vector2 playerPos = _rb.transform.position;
        Vector2 direction = (enemyPos - playerPos).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rot = Quaternion.Euler(0f, 0f, angle + 90f);

        Instantiate(_hitEffectPrefab, enemyPos, rot);
    }

    protected void Shake()
    {
        if (_isShaking || _cameraImpulseSource == null) return;
        StartCoroutine(DoShake());
    }

    protected IEnumerator DoShake()
    {
        _isShaking = true;
        _cameraImpulseSource.GenerateImpulseWithForce(shakeForce);
        yield return new WaitForSeconds(shakeCooldown);
        _isShaking = false;
    }
}
