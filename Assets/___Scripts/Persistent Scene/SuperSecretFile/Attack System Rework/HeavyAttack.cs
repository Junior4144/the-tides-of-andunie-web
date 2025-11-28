using System.Collections;
using UnityEngine;

public class HeavyAttack : BaseAttack
{
    [Header("Heavy Attack Settings")]
    [SerializeField] private float _movementSpeedIncrease = -2f;
    [SerializeField] private float _meleeDamageIncrease = 10f;
    [SerializeField] private float _audioFadeOutDuration = 0.3f;
    [SerializeField] private GameObject _deflectionCollider;

    private (float speed, float damage) _originalStats;
    private WeaponCooldownHandler _cooldownHandler;

    protected override void Awake()
    {
        base.Awake();
        _cooldownHandler = GetComponentInParent<WeaponCooldownHandler>();
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

    void StopLoopingAttackSound() =>
        StartCoroutine(FadeOutAttackSound());

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
        stats.SetMeleeDamage(_originalStats.damage + _meleeDamageIncrease);
    }

    void ResetStatBuffs()
    {
        var stats = PlayerStatsManager.Instance;
        stats.SetSpeed(_originalStats.speed);
        stats.SetMeleeDamage(_originalStats.damage);
    }

    void EndAttack()
    {
        _isAttacking = false;
        WeaponManager.Instance.SetBusy(false);
        _hitEnemies.Clear();
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
}
