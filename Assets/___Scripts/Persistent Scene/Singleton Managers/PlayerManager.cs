using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(DestroyController))]
public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }

    private PlayerHealthController _healthController;
    private PlayerController _playerMovement;
    private LSPlayerMovement _lsPlayerMovement;
    private PlayerAttackController _attackController;
    private ImpulseController _impulseController;

    public bool AllowForceChange = false;
    public bool IsInvincible { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        _healthController = GetComponentInChildren<PlayerHealthController>();
        _playerMovement = GetComponent<PlayerController>();
        _lsPlayerMovement = GetComponent<LSPlayerMovement>();
        _impulseController = GetComponent<ImpulseController>();
        _attackController = GetComponentInChildren<PlayerAttackController>();
    }

    private void OnEnable() =>
        GameManager.OnGameStateChanged += HandleGameStateChanged;

    private void OnDisable() =>
        GameManager.OnGameStateChanged -= HandleGameStateChanged;


    private void HandleGameStateChanged(GameState newState)
    {
        Debug.Log($"PlayerManager responding to new state: {newState}");

        switch (newState)
        {
            case GameState.Gameplay:
                if (!_playerMovement) return;
                _playerMovement.enabled = true;
                break;
            case GameState.Menu:
            case GameState.Paused:
            case GameState.Cutscene:
                if (_lsPlayerMovement) _lsPlayerMovement.enabled = false;

                if (_playerMovement) _playerMovement.enabled = false;
                break;
            case GameState.LevelSelector:
                _lsPlayerMovement.enabled = true;
                break;
            default:
                break;
        }
    }

    //------HEALTH------//
    public float GetCurrentHealth() => _healthController.GetCurrentHealth();

    public float GetPercentHealth() => _healthController.GetPercentHealth();

    public void SetHealth(float value) => _healthController.SetCurrentHealth(value);

    public void AddHealth(float value) => _healthController.AddHealth(value);

    public void TakeDamage(float value, DamageType damageType) => _healthController.TakeDamage(value, damageType);

    public void SetInvincible(bool invincible) => IsInvincible = invincible;
    

    //------TRANSFORM------//
    public Transform GetPlayerTransform() => gameObject.transform;

    public void SetPlayerTransform(Vector3 pos, Quaternion rotation) => gameObject.transform.SetPositionAndRotation(pos, rotation);


    //------ATTACK------//
    public bool IsAttacking() => _attackController.IsAttacking;

    public float GetDamageAmount() => PlayerStatsManager.Instance.MeleeDamage;


    //------LS PLAYER------//
    public NavMeshAgent GetPlayerAgent() => GetComponent<NavMeshAgent>();


    //------MOVEMENT------//
    public bool IsInImpulse() => _impulseController.IsInImpulse();

    public void ApplyImpulse(Vector2 contactPoint, Vector2 impulseDirection, ImpulseSettings settings) => 
        _impulseController.InitiateSquadImpulse(contactPoint, impulseDirection, settings);

    public void DisableLSPlayerMovement()
    {
        _lsPlayerMovement.enabled = false;
    }

    public void EnableLSPlayerMovement()
    {
        _lsPlayerMovement.enabled = true;
    }

    public void DisablePlayerMovement()
    {
        if (_playerMovement)
            _playerMovement.enabled = false;
    }

    public void EnablePlayerMovement()
    {
        if (_playerMovement)
            _playerMovement.enabled = true;
    }


    //------DESTROY------//
    public void HandleDestroy()
    {
        if (this == null) return;

        if (TryGetComponent<DestroyController>(out var dc))
        {
            dc.Destroy(0f);
        }
    }
}