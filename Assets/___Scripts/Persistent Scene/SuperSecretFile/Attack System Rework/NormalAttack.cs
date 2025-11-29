using System.Collections;
using UnityEngine;

public class NormalAttack : BaseAttack
{
    [Header("Normal Attack Settings")]
    [SerializeField] private float _impulseStrength = 50;
    [SerializeField] private float _impulseDuration = 0.1f;

    [Header("Attack Arc Settings")]
    [SerializeField] private float _attackArcDegrees = 120f;
    [SerializeField] private float _attackStartAngle = -60f;

    private ImpulseController _impulseController;

    protected override void Awake()
    {
        base.Awake();
        _impulseController = GetComponentInParent<ImpulseController>();
    }

    public override void Execute()
    {
        if (_isAttacking) return;

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

    protected override void OnTriggerEnter2D(Collider2D col)
    {
        if (!_isAttacking || _hitEnemies.Contains(col)) return;

        if (col.TryGetComponent(out HealthController health))
        {
            ApplyImpulse(col);
            _hitEnemies.Add(col);
            StartCoroutine(DealDamageRoutine(health));
            SpawnHitEffect(col.transform.position);
            Shake();
        }
    }

    void ApplyImpulse(Collider2D otherCollider)
    {
        if (_impulseController == null) return;

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
}
