using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class PlayerAttackController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] PlayerAnimator _animator;
    [SerializeField] AudioClip _attackSound;
    [SerializeField] GameObject _hitEffectPrefab;
    [SerializeField] GameObject _impulseCollider;

    [Header("Settings")]
    [SerializeField] float _attackDuration = 0.5f;
    [SerializeField] float _damageDelay = 0f;
    [SerializeField] private float _impulseStrength = 50;
    [SerializeField] private float _impulseDuration = 0.1f;
    [SerializeField] private float shakeCooldown = 0.2f;

    [Header("Attack Arc Settings")]
    [SerializeField] float _attackArcDegrees = 120f;
    [SerializeField] float _attackStartAngle = -60f;

    [Header("Sweep Attack Settings")]
    [SerializeField] float _sweepRotations = 3f;
    [SerializeField] float _sweepDuration = 0.3f;

    private AudioSource _audioSrc;
    private Rigidbody2D _rb;
    private ImpulseController _impulseController;
    private CinemachineImpulseSource camraImpulseSource;
    private readonly HashSet<Collider2D> _hitEnemies = new();
    bool _isAttacking;
    bool _isSweepAttacking;

    [Header("Screen Shake Settings")]
    public float force;

    [Header("HitStop Settings")]
    public float hitStopCooldown;
    private bool isInHitStop;
    public float hitStopDuration;

    public bool IsAttacking => _isAttacking;
    public float AttackDuration => _attackDuration;
    
    private bool isShaking;

    void Awake()
    {
        _audioSrc = GetComponent<AudioSource>();
        _rb = GetComponentInParent<Rigidbody2D>();
        camraImpulseSource = GetComponent<CinemachineImpulseSource>();
        _impulseController = GetComponentInParent<ImpulseController>();
        _isAttacking = false;
    }

    private void OnDisable()
    {
        StopAllCoroutines();

        _isAttacking = false;
        _isSweepAttacking = false;
        isShaking = false;
        isInHitStop = false;

        _hitEnemies.Clear();
        WeaponManager.Instance?.SetBusy(false);
        _impulseCollider.SetActive(false);
        transform.localRotation = Quaternion.identity;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !_isAttacking)
            StartAttack();

        if (Input.GetMouseButtonDown(1) && !_isAttacking)
            StartSweepAttack();

        _impulseCollider.SetActive(_isAttacking);
    }

    void StartAttack()
    {
        _isAttacking = true;
        WeaponManager.Instance.SetBusy(true);
        PlayAttackAnimation();
    }

    void PlayAttackAnimation()
    {
        _animator?.TriggerAttack();
        if (_attackSound) _audioSrc.PlayOneShot(_attackSound, 0.4f);
        StartCoroutine(AttackRoutine());
        StartCoroutine(SweepAttackCollider());
    }

    IEnumerator AttackRoutine()
    {
        yield return new WaitForSeconds(_attackDuration);
        _isAttacking = false;
        WeaponManager.Instance.SetBusy(false);
        _hitEnemies.Clear();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (!_isAttacking) return;
        if (_hitEnemies.Contains(col)) return;

        if (col.TryGetComponent(out HealthController health))
        {
            if (!_isSweepAttacking)
                ApplyImpulse(col);

            _hitEnemies.Add(col);
            StartCoroutine(DealDamage(health));
            SpawnHitEffect(col.transform.position);
            HandleHitStop();

            Shake();
        }
    }

    IEnumerator DealDamage(HealthController health)
    {
        yield return new WaitForSeconds(_damageDelay);
        
        health.TakeDamage(PlayerStatsManager.Instance.MeleeDamage);
    }

    void SpawnHitEffect(Vector2 enemyPos)
    {
        Vector2 playerPos = _rb.transform.position;
        Vector2 direction = (enemyPos - playerPos).normalized;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rot = Quaternion.Euler(0f, 0f, angle + 90f);

        Instantiate(_hitEffectPrefab, enemyPos, rot);
    }

    IEnumerator SweepAttackCollider()
    {
        float elapsed = 0f;

        while (elapsed < _attackDuration)
        {
            float progress = elapsed / _attackDuration;
            float angle = _attackStartAngle + (_attackArcDegrees * progress);

            transform.localRotation = Quaternion.Euler(0, 0, angle);

            elapsed += Time.deltaTime;
            yield return null;
        }
    }

    void StartSweepAttack()
    {
        _isAttacking = true;
        _isSweepAttacking = true;
        PlaySweepAttackAnimation();
    }

    void PlaySweepAttackAnimation()
    {
        _animator?.TriggerAttack();
        if (_attackSound) _audioSrc.PlayOneShot(_attackSound, 0.4f);
        StartCoroutine(SweepAttackRoutine());
        StartCoroutine(Sweep360Collider());
    }

    IEnumerator SweepAttackRoutine()
    {
        yield return new WaitForSeconds(_sweepDuration);
        _isAttacking = false;
        _isSweepAttacking = false;
        _hitEnemies.Clear();
    }

    IEnumerator Sweep360Collider()
    {
        float elapsed = 0f;
        float startAngle = transform.localRotation.eulerAngles.z;
        float totalAngle = _sweepRotations * 360f;

        while (elapsed < _sweepDuration)
        {
            float progress = elapsed / _sweepDuration;
            float angle = startAngle + (totalAngle * progress);

            transform.localRotation = Quaternion.Euler(0, 0, angle);

            elapsed += Time.deltaTime;
            yield return null;
        }
    }

    void ApplyImpulse(Collider2D otherCollider)
    {
        var impulseSettings = new ImpulseSettings
        {
            Force = _impulseStrength,
            Duration = _impulseDuration,
            PlaySound = true,
            SpawnParticles = true
        };

        _impulseController.InitiateSquadImpulse(
            contactPoint: otherCollider.ClosestPoint(transform.position),
            impulseDirection: _rb.transform.up,
            impulseSettings
        );
    }

    public void Shake()
    {
        if (isShaking)
            return;

        StartCoroutine(DoShake(force));
    }

    private IEnumerator DoShake(float force)
    {
        isShaking = true;
        camraImpulseSource.GenerateImpulseWithForce(force);
        yield return new WaitForSeconds(shakeCooldown);
        isShaking = false;
    }

    public void HandleHitStop()
    {
        if (isInHitStop)
            return;

        StartCoroutine(DoHitStop());
    }

    private IEnumerator DoHitStop()
    {
        isInHitStop = true;
        HitStopManager.Instance.Stop(hitStopDuration);
        yield return new WaitForSeconds(hitStopCooldown);
        isInHitStop = false;
    }
}
