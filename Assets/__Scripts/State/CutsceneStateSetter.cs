using UnityEngine;

public class CutsceneStateSetter : MonoBehaviour
{
    public enum UIType { None, Gameplay, Cutscene, MainMenu }
    void Start()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.SetState(GameState.Cutscene);
    }
}