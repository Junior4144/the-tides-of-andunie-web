using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }

    private IHealthController healthController;

    private PlayerHeroMovement _playerMovement;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        healthController = GetComponentInChildren<IHealthController>();
        _playerMovement = GetComponent<PlayerHeroMovement>();
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
                _playerMovement.enabled = false;
                break;
            default:
                break;
        }
    }

    public float GetHealth() => healthController.GetCurrentHealth();
    public float GetMaxHealth() => healthController.GetMaxHealth();
    public float GetPercentHealth() => healthController.GetPercentHealth();
    public float GetDamageAmount() => PlayerStatsManager.Instance.MeleeDamage;
    public Transform GetPlayerTransform() => gameObject.transform;
    
    public void SetPlayerTransform(Vector3 pos, Quaternion rotation) => gameObject.transform.SetPositionAndRotation(pos, rotation);
    public void SetHealth(float value) => healthController.SetCurrentHealth(value);
    public void AddHealth(float value) => healthController.AddHealth(value);
    public void HandleDestroy() => GetComponent<DestroyController>().Destroy(0f);    
}