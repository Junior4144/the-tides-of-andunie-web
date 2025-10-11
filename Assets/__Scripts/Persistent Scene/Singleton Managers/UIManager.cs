using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Tooltip("The root gameplay canvas (health bar, etc.)")]
    public GameObject gameplayCanvas;

    [Header("UI Groups")]
    [SerializeField] private GameObject gameplayUI;

    private void Awake()
    {
        if (Instance != null) return;

        Instance = this;
        HideAll();
    }
    private void OnEnable() =>
        // Subscribe to GameManager event
        GameManager.OnGameStateChanged += HandleGameStateChanged;

    private void Start() =>
        HandleGameStateChanged(GameManager.Instance.CurrentState);

    private void HandleGameStateChanged(GameState newState)
    {
        HideAll();
        Debug.Log($"UIManager responding to new state: {newState}");

        switch (newState)
        {
            case GameState.Gameplay:
                gameplayUI.SetActive(true);
                break;

            case GameState.Menu:
            case GameState.Paused:
            case GameState.Cutscene:
            default:
                break;
        }
    }

    private void HideAll()
    {
        gameplayUI.SetActive(false);
    }
}
