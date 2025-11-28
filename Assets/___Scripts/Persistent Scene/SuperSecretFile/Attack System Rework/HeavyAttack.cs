using UnityEngine;

public class HeavyAttack : BaseAttack
{
    [Header("Heavy Attack Settings")]
    [SerializeField] private AudioClip _whirlwindSound;
    [SerializeField] private float _movementSpeedIncrease = -2f;
    [SerializeField] private float _meleeDamageIncrease = 10f;

    private (float speed, float damage) _originalStats;

    public override void Execute()
    {
        if (_isAttacking) return;

        BeginHeavyAttack();
        Invoke(nameof(CompleteHeavyAttack), _attackDuration);
    }

    void BeginHeavyAttack()
    {
        _isAttacking = true;
        WeaponManager.Instance.SetBusy(true);

        _animator?.TriggerHeavyAttack();
        if (_whirlwindSound) _audioSrc.PlayOneShot(_whirlwindSound, 0.4f);

        ApplyStatBuffs();
    }

    void CompleteHeavyAttack()
    {
        ResetStatBuffs();
        EndAttack();
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
