using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Tooltip("The root gameplay canvas (health bar, etc.)")]
    public GameObject gameplayCanvas;

    [Header("UI Groups")]
    [SerializeField] private GameObject gameplayUI;
    [SerializeField] private HealthBarUI _healthBarUI;
    [SerializeField] private HealthBarShake _healthBarShake;
    [SerializeField] private GameObject _InventoryUI;

    private void Awake()
    {
        if (Instance != null) return;

        Instance = this;
        HideAll();
    }
    private void OnEnable()
    {
        GameManager.OnGameStateChanged += HandleGameStateChanged;
        PlayerHealthController.OnHealthChanged += HandleHealthChanged;
    }

    private void Start() =>
        HandleGameStateChanged(GameManager.Instance.CurrentState);

    private void HandleGameStateChanged(GameState newState)
    {
        HideAll();
        Debug.Log($"UIManager responding to new state: {newState}");

        switch (newState)
        {
            case GameState.Gameplay:
                _InventoryUI.SetActive(true);
                gameplayUI.SetActive(true);
                break;

            case GameState.Menu:
            case GameState.Paused:
            case GameState.Cutscene:
            default:
                break;
        }
    }
    private void HandleHealthChanged(float _currentHealth, float _maxhealth)
    {
        if (!_healthBarShake.gameObject.activeSelf) return;

        _healthBarShake.Shake();
        _healthBarUI.UpdateHealthBar(_currentHealth, _maxhealth);

    }
    
    public void UpdateHealthBar(float _currentHealth, float _maxhealth)
    {
        if (!_healthBarUI.gameObject.activeSelf) return;

        _healthBarUI.UpdateHealthBar(_currentHealth, _maxhealth);
    }

    private void HideAll()
    {
        _InventoryUI.SetActive(false);
        gameplayUI.SetActive(false);
    }
    public bool Check_HealthBar_UI_IsActive() => _healthBarUI.gameObject.activeInHierarchy;
}
