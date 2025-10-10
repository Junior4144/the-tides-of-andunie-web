using UnityEngine;

public class GameplayStateSetter : MonoBehaviour
{
    void Start()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.SetState(GameState.Gameplay);
    }
}