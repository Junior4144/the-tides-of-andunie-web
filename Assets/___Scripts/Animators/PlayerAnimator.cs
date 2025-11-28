using System;
using System.Collections;
using System.Linq;
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
    private HeavyAttackPhase _heavyAttackPhase = HeavyAttackPhase.None;
    private BowState _currentBowState = BowState.None;
    private float _nextIdleCheckTime;
    private bool _playingSpecialIdle;
    private float _specialIdleEndTime;
    private int _currentSpecialIdleState;

    private AnimationClip _heavyTwirlStartClip;
    private AnimationClip _heavyTwirlEndClip;

    private enum BowState { None, HandleIdle, Charging, ChargeIdle }
    private enum HeavyAttackPhase { None, Start, Loop, End }
    
    private void Awake()
    {
        _anim = GetComponent<Animator>();
        _playerMovement = GetComponentInParent<PlayerController>();
        _nextIdleCheckTime = Time.time + UnityEngine.Random.Range(_minIdleInterval, _maxIdleInterval);

        var clips = _anim.runtimeAnimatorController.animationClips;
        _heavyTwirlStartClip = clips.FirstOrDefault(c => c.name == "AldarionHeavyTwirlStart");
        _heavyTwirlEndClip = clips.FirstOrDefault(c => c.name == "AldarionHeavyTwirlEnd");
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
        if (_currentBowState != BowState.None) return;

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

        if (_heavyAttackPhase != HeavyAttackPhase.None)
        {
            _playingSpecialIdle = false;
            return GetHeavyAttackAnimation();
        }

        if (_attacked)
        {
            _playingSpecialIdle = false;
            return LockState(Attack, _attackAnimDuration);
        }

        if (_currentBowState != BowState.None)
            return GetBowStateAnimation();

        if (_playingSpecialIdle)
            return _currentSpecialIdleState;

        return IdleDefault;

        int LockState(int s, float t)
        {
            _lockedTill = Time.time + t;
            return s;
        }
    }

    private int GetHeavyAttackAnimation() => _heavyAttackPhase switch
    {
        HeavyAttackPhase.Start => HeavyAttackStart,
        HeavyAttackPhase.Loop => HeavyAttackLoop,
        HeavyAttackPhase.End => HeavyAttackEnd,
        _ => IdleDefault
    };

    private int GetBowStateAnimation() => _currentBowState switch
    {
        BowState.HandleIdle => BowHandleIdle,
        BowState.Charging => BowCharge,
        BowState.ChargeIdle => BowChargeIdle,
        _ => IdleDefault
    };
    
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
        _currentBowState = BowState.None;
    }

    public void TriggerHeavyAttack(float totalDuration)
    {
        _heavyAttackPhase = HeavyAttackPhase.Start;
        _currentBowState = BowState.None;

        float startDuration = _heavyTwirlStartClip.length;
        float endDuration = _heavyTwirlEndClip.length;
        float loopDuration = totalDuration - startDuration - endDuration;

        Invoke(nameof(TransitionToHeavyLoop), startDuration);
        Invoke(nameof(TransitionToHeavyEnd), startDuration + loopDuration);
        Invoke(nameof(CompleteHeavyAttack), totalDuration);
    }

    void TransitionToHeavyLoop() => _heavyAttackPhase = HeavyAttackPhase.Loop;
    void TransitionToHeavyEnd() => _heavyAttackPhase = HeavyAttackPhase.End;
    void CompleteHeavyAttack() => _heavyAttackPhase = HeavyAttackPhase.None;

    public void TriggerBowHandleIdle() =>
        _currentBowState = BowState.HandleIdle;

    public void TriggerBowCharge() =>
        _currentBowState = BowState.Charging;

    public void TriggerBowChargeIdle() =>
        _currentBowState = BowState.ChargeIdle;

    public void ReturnToDefaultIdle() =>
        _currentBowState = BowState.None;
    
    #region Cached Properties
    private int _currentState;
    private static readonly int IdleDefault = Animator.StringToHash("AldarionIdle");
    private static readonly int IdleAngry = Animator.StringToHash("AldarionIdleAngry");
    private static readonly int IdleAxe = Animator.StringToHash("AldarionIdleAxe");
    private static readonly int IdleWind = Animator.StringToHash("AldarionIdleWind");
    private static readonly int Attack = Animator.StringToHash("AldarionSlash");
    private static readonly int HeavyAttackStart = Animator.StringToHash("AldarionHeavyTwirlStart");
    private static readonly int HeavyAttackLoop = Animator.StringToHash("AldarionHeavyTwirlLoop");
    private static readonly int HeavyAttackEnd = Animator.StringToHash("AldarionHeavyTwirlEnd");
    private static readonly int BowHandleIdle = Animator.StringToHash("AldarionBowHandleIdle");
    private static readonly int BowCharge = Animator.StringToHash("AldarionBowCharge");
    private static readonly int BowChargeIdle = Animator.StringToHash("AldarionBowChargeIdle");
    #endregion
}