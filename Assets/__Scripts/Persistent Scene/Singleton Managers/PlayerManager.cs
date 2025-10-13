using UnityEngine;


public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }

    private IHealthController healthController;
    private MeleeController meleeController;

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
        meleeController = GetComponentInChildren<MeleeController>();
        _playerMovement = GetComponent<PlayerHeroMovement>();
    }
    private void Start() =>
        SaveManager.Instance.InitializeDefaultSave();


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
    public float GetDamageAmount() => meleeController.GetDamageAmount();

    public void SetHealth(float value) => healthController.SetCurrentHealth(value);
    public void SetDamageAmount(float value) => meleeController.SetDamageAmount(value);

    public void HandleDestroy() => GetComponent<DestroyController>().Destroy(0f);
}