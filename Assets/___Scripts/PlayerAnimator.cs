using System;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{

    [SerializeField] private float _attackAnimDuration = 0.5f;
    [SerializeField] private float _minIdleInterval = 5f;
    [SerializeField] private float _maxIdleInterval = 15f;
    [SerializeField] [Range(0f, 1f)] private float _specialIdleChance = 0.3f;
    [SerializeField] private float _idleAngryDuration = 1f;
    [SerializeField] private float _idleAxeDuration = 1f;
    [SerializeField] private float _idleWindDuration = 1f;

    private Animator _anim;
    private PlayerHeroMovement _playerMovement;
    private float _lockedTill;
    private bool _attacked;
    private float _nextIdleCheckTime;
    private bool _playingSpecialIdle;
    private float _specialIdleEndTime;
    private int _currentSpecialIdleState;
    
    private void Awake()
    {
        _anim = GetComponent<Animator>();
        _playerMovement = GetComponentInParent<PlayerHeroMovement>();
        _nextIdleCheckTime = Time.time + UnityEngine.Random.Range(_minIdleInterval, _maxIdleInterval);
    }
    
    public void TriggerAttack()
    {
        _attacked = true;
    }

    private void Update()
    {
        HandleSpecialIdle();

        var state = GetState();
        _attacked = false;

        if (state == _currentState) return;

        _anim.CrossFade(state, 0, 0);
        _currentState = state;
    }
    
    private void HandleSpecialIdle()
    {
        if (_playingSpecialIdle && Time.time >= _specialIdleEndTime)
            _playingSpecialIdle = false;

        if (
            Time.time >= _nextIdleCheckTime &&
            Time.time >= _lockedTill &&
            !_playingSpecialIdle &&
            !_playerMovement.IsWalking
        )
        {
            if (UnityEngine.Random.value <= _specialIdleChance)
            {
                _playingSpecialIdle = true;
                _currentSpecialIdleState = PickRandomSpecialIdle();
            }
            _nextIdleCheckTime = Time.time + UnityEngine.Random.Range(_minIdleInterval, _maxIdleInterval);
        }
    }
    
    private int GetState()
    {
        if (Time.time < _lockedTill) return _currentState;
        
        if (_attacked)
        {
            _playingSpecialIdle = false;
            return LockState(Attack, _attackAnimDuration);
        }
        
        if (_playingSpecialIdle)
            return _currentSpecialIdleState;
        
        return IdleDefault;
        
        int LockState(int s, float t)
        {
            _lockedTill = Time.time + t;
            return s;
        }
    }
    
    private int PickRandomSpecialIdle()
    {
        int random = UnityEngine.Random.Range(0, 3);
        
        float duration = random switch
        {
            0 => _idleAngryDuration,
            1 => _idleAxeDuration,
            2 => _idleWindDuration,
            _ => _idleAngryDuration
        };
        
        _specialIdleEndTime = Time.time + duration;
        
        return random switch
        {
            0 => IdleAngry,
            1 => IdleAxe,
            2 => IdleWind,
            _ => IdleAngry
        };
    }
    
    #region Cached Properties
    private int _currentState;
    private static readonly int IdleDefault = Animator.StringToHash("AldarionIdle");
    private static readonly int IdleAngry = Animator.StringToHash("AldarionIdleAngry");
    private static readonly int IdleAxe = Animator.StringToHash("AldarionIdleAxe");
    private static readonly int IdleWind = Animator.StringToHash("AldarionIdleWind");
    private static readonly int Attack = Animator.StringToHash("AldarionSlash");
    #endregion
}