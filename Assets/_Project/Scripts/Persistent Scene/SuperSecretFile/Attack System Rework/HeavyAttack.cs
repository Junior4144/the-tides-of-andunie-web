using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavyAttack : BaseAttack
{
    [Header("Heavy Attack Settings")]
    [SerializeField] private float _movementSpeedIncrease = -2f;
    [SerializeField] private float _meleeDamageMultiplier = 0.7f;
    [SerializeField] private float _audioFadeOutDuration = 0.3f;
    [SerializeField] private GameObject _deflectionCollider;
    [SerializeField] private float _damageTickInterval = 0.1f; // Time between continuous damage hits

    private (float speed, float damage) _originalStats;
    private WeaponCooldownHandler _cooldownHandler;
    private Dictionary<Collider2D, float> _lastHitTimes = new Dictionary<Collider2D, float>();

    private bool _hasAppliedBuffs = false;

    protected override void Awake()
    {
        base.Awake();
        _cooldownHandler = GetComponentInParent<WeaponCooldownHandler>();
    }

    protected override void OnDisable()
    {

        ResetAllPlayerChanges();

        if (_animator != null)
            _animator.CancelHeavyAttackAnimation();

        base.OnDisable();
    }

    public override void Execute()
    {
        if (_isAttacking) return;
        if (_cooldownHandler.IsAbilityOnCooldown) return;

        _cooldownHandler.StartAbilityCooldown();
        BeginHeavyAttack();
        Invoke(nameof(CompleteHeavyAttack), _attackDuration);
    }

    void BeginHeavyAttack()
    {
        _isAttacking = true;
        WeaponManager.Instance.SetBusy(true);
        PlayerManager.Instance.SetInvincible(true);

        if (_deflectionCollider != null)
            _deflectionCollider.SetActive(true);

        _animator?.TriggerHeavyAttack(_attackDuration);
        PlayLoopingAttackSound();

        ApplyStatBuffs();
    }

    void CompleteHeavyAttack()
    {
        if (!isActiveAndEnabled) return;   // <--- add safety check

        if (_deflectionCollider != null)
            _deflectionCollider.SetActive(false);

        StopLoopingAttackSound();
        ResetStatBuffs();
        PlayerManager.Instance.SetInvincible(false);
        EndAttack();
    }

    void PlayLoopingAttackSound()
    {
        if (_attackSound == null) return;

        _audioSrc.clip = _attackSound;
        _audioSrc.loop = true;
        _audioSrc.Play();
    }

    void StopLoopingAttackSound()
    {
        if (!isActiveAndEnabled)   // <- critical fix
            return;

        StartCoroutine(FadeOutAttackSound());
    }

    IEnumerator FadeOutAttackSound()
    {
        float startVolume = _audioSrc.volume;
        float elapsed = 0f;

        while (elapsed < _audioFadeOutDuration)
        {
            elapsed += Time.deltaTime;
            _audioSrc.volume = Mathf.Lerp(startVolume, 0f, elapsed / _audioFadeOutDuration);
            yield return null;
        }

        _audioSrc.loop = false;
        _audioSrc.Stop();
        _audioSrc.volume = startVolume;
    }

    void ApplyStatBuffs()
    {
        var stats = PlayerStatsManager.Instance;
        _originalStats = (stats.Speed, stats.MeleeDamage);

        stats.SetSpeed(_originalStats.speed + _movementSpeedIncrease);
        stats.SetMeleeDamage(_originalStats.damage * _meleeDamageMultiplier);

        _hasAppliedBuffs = true;  // <-- track it
    }

    void ResetStatBuffs()
    {
        if (!_hasAppliedBuffs) return;  // <-- prevents zeroing out on disable

        var stats = PlayerStatsManager.Instance;
        stats.SetSpeed(_originalStats.speed);
        stats.SetMeleeDamage(_originalStats.damage);

        _hasAppliedBuffs = false;  // <-- clear state
    }

    void EndAttack()
    {
        _isAttacking = false;
        WeaponManager.Instance.SetBusy(false);
        _hitEnemies.Clear();
        _lastHitTimes.Clear();
    }

    private void ResetAllPlayerChanges()
    {
        // ONLY reset if actual heavy attack was active
        if (_isAttacking)
        {
            ResetStatBuffs();
            PlayerManager.Instance.SetInvincible(false);
            EndAttack();
        }

        // Reset collider only if used
        if (_deflectionCollider != null)
            _deflectionCollider.SetActive(false);

        // Reset audio — safe early exit inside StopLoopingAttackSound
        StopLoopingAttackSound();
    }

    protected override void OnTriggerEnter2D(Collider2D col)
    {
        if (!_isAttacking) return;

        if (col.TryGetComponent(out HealthController health))
        {
            StartCoroutine(DealDamageRoutine(health));
            SpawnHitEffect(col.transform.position);
            Shake();
        }
    }

    protected void OnTriggerStay2D(Collider2D col)
    {
        if (!_isAttacking) return;
        if (!col.TryGetComponent(out HealthController health)) return;

        if (!_lastHitTimes.TryGetValue(col, out float lastHitTime) ||
            Time.time >= lastHitTime + _damageTickInterval)
        {
            _lastHitTimes[col] = Time.time;
            StartCoroutine(DealDamageRoutine(health));
            SpawnHitEffect(col.transform.position);
            Shake();
        }
    }
}
