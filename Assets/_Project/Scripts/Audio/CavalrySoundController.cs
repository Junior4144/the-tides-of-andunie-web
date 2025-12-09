using System;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

[RequireComponent(typeof(CavalryMovementController))]
public class CavalrySoundController : MonoBehaviour
{

    [Header("Audio Sources")]
    [Tooltip("This source should have 'Loop' checked.")]
    [SerializeField] private AudioSource _runLoopSource; 
    [Tooltip("This source is for one-shot sounds like hits/charges.")]
    [SerializeField] private AudioSource _eventSource;

    [Header("Clips")]
    [SerializeField] private AudioClip[] _chargeStartClips;
    [SerializeField] private AudioClip[] _playerHitClips;

    [Header("Pitch Settings")]
    [SerializeField] private float _minPitch = 0.75f;
    [SerializeField] private float _maxPitch = 1.25f;
    [Tooltip("Slight random pitch shift to make each horse sound unique.")]
    [Range(-0.2f, 0.2f)]
    [SerializeField] private float _minRandomPitchOffset = -0.05f;
    [Range(-0.2f, 0.2f)]
    [SerializeField] private float _maxRandomPitchOffset = 0.05f;

    [Header("Cooldowns")]
    [Tooltip("How long to wait before another charge sound can play.")]
    [SerializeField] private float _chargeSoundCooldown = 5.0f;


    private CavalryMovementController _movementController;
    private CavalryMeleeController _meleeController;

    private float _lastChargeSoundTime;
    private float _pitchOffset;

    void Awake()
    {
        _runLoopSource.clip?.LoadAudioData();
        if (_chargeStartClips != null) foreach (var clip in _chargeStartClips) clip.LoadAudioData();
        if (_playerHitClips != null) foreach (var clip in _playerHitClips) clip.LoadAudioData();
        _movementController = gameObject.GetComponent<CavalryMovementController>();
        _meleeController = gameObject.GetComponentInChildren<CavalryMeleeController>();
    }


    private void OnEnable()
    {
        _movementController.OnChargeStart += PlayChargeSound;
        _meleeController.OnAttack += PlayHitSound;
    }

    private void OnDisable()
    {
        _movementController.OnChargeStart -= PlayChargeSound;
        _meleeController.OnAttack -= PlayHitSound;
    }

    private void PlayChargeSound()
    {
        if (Time.time - _lastChargeSoundTime > _chargeSoundCooldown){
            PlayRandomClipOneShot(_chargeStartClips);
            _lastChargeSoundTime = Time.time;
        }
    }

    private void PlayHitSound()
    {
        PlayRandomClipOneShot(_playerHitClips);
    }

    private void Start()
    {
        _lastChargeSoundTime = -_chargeSoundCooldown;
        _pitchOffset = Random.Range(_minRandomPitchOffset, _maxRandomPitchOffset);

        if (_runLoopSource.clip != null)
        {
            _runLoopSource.time = Random.Range(0f, _runLoopSource.clip.length);
        }
    }

    private void Update()
    {
        HandleRunLoop();
    }

    private void HandleRunLoop()
    {
        float speedPercent = Mathf.InverseLerp(0, _movementController.MaxSpeed, _movementController.CurrentSpeed);

        if (speedPercent > 0.1f)
        {
            if (!_runLoopSource.isPlaying)
                _runLoopSource.Play();
            

            _runLoopSource.pitch = Mathf.Lerp(_minPitch, _maxPitch, speedPercent) + _pitchOffset;
        }
        else if (_runLoopSource.isPlaying)
                _runLoopSource.Stop();
    }

    private void PlayRandomClipOneShot(AudioClip[] clips)
    {
        if (ClipsExist(clips))
        {
            Debug.LogWarning("Audio array is empty, not playing sound.");
            return;
        }

        int index = Random.Range(0, clips.Length);
        AudioClip clipToPlay = clips[index];

        if (clipToPlay != null)
        {
            _eventSource.PlayOneShot(clipToPlay);
        }
    }

    private bool ClipsExist(AudioClip[] clips) => clips != null && clips.Length == 0;
}