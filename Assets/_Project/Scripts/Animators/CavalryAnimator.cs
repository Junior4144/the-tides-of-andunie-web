using UnityEngine;

[RequireComponent(typeof(Animator))]
public class CavalryAnimator : MonoBehaviour
{
    [SerializeField] private float _attackAnimDuration = 0.75f;

    private Animator _animator;
    private float _lockedTill;
    public bool _attacked;
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
        // var x = _attacked? "Attack": "Run";
        // Debug.Log($"Cavalry Anim State is now {x}");
        
        if (_attacked)
            return LockState(Attack, _attackAnimDuration);
        
        return Run;
    }

    private int LockState(int s, float t)
    {
        _lockedTill = Time.time + t;
        return s;
    }
    
    #region Cached Properties
    private static readonly int Run = Animator.StringToHash("Run");
    private static readonly int Attack = Animator.StringToHash("Attack");
    #endregion
}