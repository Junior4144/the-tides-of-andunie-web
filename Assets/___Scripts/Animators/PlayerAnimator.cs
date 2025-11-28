using System;
using System.Collections;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{

    [SerializeField] private float _attackAnimDuration = 0.5f;
    [SerializeField] private float _minIdleInterval = 5f;
    [SerializeField] private float _maxIdleInterval = 15f;
    //[SerializeField] [Range(0f, 1f)] private float _specialIdleChance = 0.3f;
    [SerializeField] private float _idleAngryDuration = 1f;
    [SerializeField] private float _idleAxeDuration = 1f;
    [SerializeField] private float _idleWindDuration = 1f;

    private Animator _anim;
    private PlayerController _playerMovement;
    private float _lockedTill;
    private bool _attacked;
    private bool _heavyAttacked;
    private float _heavyAttackDuration;
    private bool _bowAttack;
    private float _nextIdleCheckTime;
    private bool _playingSpecialIdle;
    private float _specialIdleEndTime;
    private int _currentSpecialIdleState;
    
    private void Awake()
    {
        _anim = GetComponent<Animator>();
        _playerMovement = GetComponentInParent<PlayerController>();
        _nextIdleCheckTime = Time.time + UnityEngine.Random.Range(_minIdleInterval, _maxIdleInterval);
    }

    private void Update()
    {
        HandleSpecialIdle();

        var state = GetState();
        _attacked = false;
        _heavyAttacked = false;

        if (state == _currentState) return;

        _anim.CrossFade(state, 0, 0);
        _currentState = state;
    }
    
    private void HandleSpecialIdle()
    {
        if (_bowAttack) return;

        if (_playingSpecialIdle && Time.time >= _specialIdleEndTime)
            _playingSpecialIdle = false;

        // TODO add IsWalking to new PlayerController
        // if (
        //     Time.time >= _nextIdleCheckTime &&
        //     Time.time >= _lockedTill &&
        //     !_playingSpecialIdle &&
        //     (_playerMovement == null || !_playerMovement.IsWalking)
        // )
        // {
        //     if (UnityEngine.Random.value <= _specialIdleChance)
        //     {
        //         _playingSpecialIdle = true;
        //         _currentSpecialIdleState = PickRandomSpecialIdle();
        //     }
        //     _nextIdleCheckTime = Time.time + UnityEngine.Random.Range(_minIdleInterval, _maxIdleInterval);
        // }
    }
    
    private int GetState()
    {
        if (Time.time < _lockedTill) return _currentState;

        if (_heavyAttacked)
        {
            _playingSpecialIdle = false;
            return LockState(HeavyAttack, _heavyAttackDuration);
        }

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
    public void TriggerAttack()
    {
        _attacked = true;
        _bowAttack = false;
    }

    public void TriggerHeavyAttack(float duration)
    {
        _heavyAttacked = true;
        _heavyAttackDuration = duration;
        _bowAttack = false;
    }
    
    
    public void PlayBowHandleIdle()
    {
        _bowAttack = true;
        _anim.CrossFade(BowHandleIdle, 0f);
    }

    public void PlayBowCharge()
    {
        _bowAttack = true;
        _anim.CrossFade(BowCharge, 0f);
    }
        
    public void PlayBowChargeIdle() => _anim.CrossFade(BowChargeIdle, 0f);

    public void ReturnToDefaultIdle()
    {
        _bowAttack = false;              // allow special idles again
        _anim.CrossFade(IdleDefault, 0f); // play the normal idle animation
    }
    
    #region Cached Properties
    private int _currentState;
    private static readonly int IdleDefault = Animator.StringToHash("AldarionIdle");
    private static readonly int IdleAngry = Animator.StringToHash("AldarionIdleAngry");
    private static readonly int IdleAxe = Animator.StringToHash("AldarionIdleAxe");
    private static readonly int IdleWind = Animator.StringToHash("AldarionIdleWind");
    private static readonly int Attack = Animator.StringToHash("AldarionSlash");
    private static readonly int HeavyAttack = Animator.StringToHash("AldarionHeavyTwirlLoop");
    private static readonly int BowHandleIdle = Animator.StringToHash("AldarionBowHandleIdle");
    private static readonly int BowCharge = Animator.StringToHash("AldarionBowCharge");
    private static readonly int BowChargeIdle = Animator.StringToHash("AldarionBowChargeIdle");
    #endregion
}