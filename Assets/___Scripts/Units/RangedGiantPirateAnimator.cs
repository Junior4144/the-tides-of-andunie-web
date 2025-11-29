using UnityEngine;

[RequireComponent(typeof(Animator))]
public class GiantRangedPirateAnimator : MonoBehaviour, IMeleeAnimator
{
    [SerializeField] private float _meleeAttackAnimDuration = 1.067f;
    [SerializeField] private float _cannonAttackAnimDuration = 0.9f;

    private Animator _animator;
    private float _lockedTill;
    private bool _meleeAttacked;
    private bool _cannonAttacked;
    private int _currentState;

    private void Awake() => _animator = GetComponent<Animator>();

    public void TriggerMeleeAttack()
    {
        _meleeAttacked = true;
        Debug.Log("[GiantRangedPirateAnimator] TriggerMeleeAttack called");
    }

    public void TriggerCannonAttack()
    {
        _cannonAttacked = true;
        Debug.Log("[GiantRangedPirateAnimator] TriggerCannonAttack called");
    }

    private void Update()
    {
        var state = GetState();
        _meleeAttacked = false;
        _cannonAttacked = false;

        if (state == _currentState) return;

        _animator.CrossFade(state, 0, 0);
        _currentState = state;
    }

    private int GetState()
    {
        if (Time.time < _lockedTill) return _currentState;

        if (_cannonAttacked)
            return LockState(Cannon_Attack, _cannonAttackAnimDuration);

        if (_meleeAttacked)
            return LockState(Left_Attack, _meleeAttackAnimDuration);

        return Idle;
    }

    private int LockState(int s, float t)
    {
        _lockedTill = Time.time + t;
        return s;
    }

    #region Cached Properties
    private static readonly int Idle = Animator.StringToHash("Idle");
    private static readonly int Left_Attack = Animator.StringToHash("LeftAttack");
    private static readonly int Cannon_Attack = Animator.StringToHash("CannonAttack");
    #endregion
}
