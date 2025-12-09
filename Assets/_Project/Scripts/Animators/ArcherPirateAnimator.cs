using UnityEngine;

[RequireComponent(typeof(Animator))]
public class ArcherPirateAnimator : MonoBehaviour
{
    [SerializeField] private float _attackAnimDuration = 0f;

    private Animator _animator;
    private float _lockedTill;
    private bool _attacked;
    private int _currentState;

    private void Awake() => _animator = GetComponent<Animator>();

    public void TriggerAttack() => _attacked = true;

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

        return Idle;
    }

    private int LockState(int s, float t)
    {
        _lockedTill = Time.time + t;
        return s;
    }

    #region Cached Properties
    private static readonly int Idle = Animator.StringToHash("Idle");
    private static readonly int Attack = Animator.StringToHash("Shooting");
    #endregion
}