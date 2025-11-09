using UnityEngine;
using System.Linq;

public class EnemySquadImpulseController : BaseSquadImpulseController
{
    [Header("Enemy-Specific Settings")]
    [SerializeField] private float _dashMultiplier = 3f;
    [SerializeField] private float _attackMultiplier = 3f;

    protected override void Awake()
    {
        base.Awake();

        _heroRigidBody = GetComponentsInChildren<UnitIdentifier>()
            .First(unit => unit.IsLeader)
            .GetComponent<Rigidbody2D>();
    }

    protected override float GetDashMultiplier(bool isDashing)
    {
        return isDashing ? _dashMultiplier : 1f;
    }

    protected override float GetAttackMultiplier(bool isAttacking)
    {
        return isAttacking ? _attackMultiplier : 1f;
    }
}
