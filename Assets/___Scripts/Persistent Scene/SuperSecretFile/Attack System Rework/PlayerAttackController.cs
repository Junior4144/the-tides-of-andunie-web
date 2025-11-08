using System.Collections;
using System.Collections.Generic;
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

    [Header("Attack Arc")]
    [SerializeField] float _attackArcDegrees = 120f;
    [SerializeField] float _attackStartAngle = -60f;

    private AudioSource _audioSrc;
    private Rigidbody2D _rb;
    private readonly HashSet<Collider2D> _hitEnemies = new();

    bool isAttacking;

    public bool IsAttacking => isAttacking;
    public float AttackDuration => _attackDuration;

    void Awake()
    {
        _audioSrc = GetComponent<AudioSource>();
        _rb = GetComponentInParent<Rigidbody2D>();
        isAttacking = false;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isAttacking)
            StartAttack();

        _impulseCollider.SetActive(isAttacking);
    }

    void StartAttack()
    {
        isAttacking = true;
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
        isAttacking = false;
        _hitEnemies.Clear();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (!isAttacking) return;
        if (_hitEnemies.Contains(col)) return;

        
        if (col.TryGetComponent(out IHealthController health))
        {
            _hitEnemies.Add(col);
            StartCoroutine(DealDamage(health));
            SpawnHitEffect(col.transform.position);
        } 
    }

    IEnumerator DealDamage(IHealthController health)
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
}
