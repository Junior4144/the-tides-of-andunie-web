using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Tooltip("The root gameplay canvas (health bar, etc.)")]
    public GameObject gameplayCanvas;

    void OnEnable()
    {
        GameManager.OnGameStateChanged += HandleGameStateChanged;
    }

    void OnDisable()
    {
        GameManager.OnGameStateChanged -= HandleGameStateChanged;
    }

    private void HandleGameStateChanged(GameState newState)
    {
        if (gameplayCanvas == null) return;

        gameplayCanvas.SetActive(newState == GameState.Gameplay);
    }
}
