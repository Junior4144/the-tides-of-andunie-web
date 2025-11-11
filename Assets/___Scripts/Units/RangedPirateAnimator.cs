using UnityEngine;

public class RangedPirateAnimator : MonoBehaviour
{
    [SerializeField] private float _attackAnimDuration = .67f;
    [SerializeField] private float _holdFireAnimDuration = 0.5f;

    private Animator _animator;
    private float _lockedTill;
    private bool _attacked;
    private bool _playerInRange;
    private int _currentState;

    private void Awake() => _animator = GetComponent<Animator>();

    public void TriggerAttack() => _attacked = true;

    public void SetPlayerInRange(bool inRange) => _playerInRange = inRange;

    private void Update()
    {
        var state = GetState();
        _attacked = false;

        if (state == _currentState) return;

        _animator.CrossFade(state, 0, 0);
        _currentState = state;
    }

    private int GetState()
    {
        if (Time.time < _lockedTill) return _currentState;

        if (_attacked)
            return LockState(Attack, _attackAnimDuration);

        if (_playerInRange)
            return LockState(FireHold, _holdFireAnimDuration);

        return Idle;
    }

    private int LockState(int s, float t)
    {
        _lockedTill = Time.time + t;
        return s;
    }

    #region Cached Properties
    private static readonly int Idle = Animator.StringToHash("RangedPirateIdle");
    private static readonly int Attack = Animator.StringToHash("RangedPirateShooting");
    private static readonly int FireHold = Animator.StringToHash("RangedPirateFireHold");
    #endregion
}
