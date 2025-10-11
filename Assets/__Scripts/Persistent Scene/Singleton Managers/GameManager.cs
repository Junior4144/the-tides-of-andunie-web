using UnityEngine;

public enum GameState
{
    Gameplay,
    Cutscene,
    Paused,
    Menu
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameState CurrentState { get; private set; } = GameState.Menu;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void SetState(GameState newState)
    {
        CurrentState = newState;
        Debug.Log($"Game state changed to: {newState}");
        OnGameStateChanged?.Invoke(newState);
    }

    public delegate void GameStateChanged(GameState newState);
    public static event GameStateChanged OnGameStateChanged;

}
