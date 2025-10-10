using UnityEngine;

public class CutsceneStateSetter : MonoBehaviour
{
    void Start()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.SetState(GameState.Cutscene);
    }
}