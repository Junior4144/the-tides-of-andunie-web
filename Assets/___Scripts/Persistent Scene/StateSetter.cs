using UnityEngine;
using UnityEngine.SceneManagement;

public class StateSetter : MonoBehaviour
{
    [SerializeField]
    private GameState stateToSet;

    void Update()
    {
        // Get the active scene
        Scene activeScene = SceneManager.GetActiveScene();
        Debug.Log("[State Setter]");
        // Check if THIS object is in that scene
        if (gameObject.scene == activeScene)
        {

            if (GameManager.Instance != null)
                GameManager.Instance.SetState(stateToSet);
            Debug.Log($"[State Setter] {activeScene.name} is active and  now SetState to {stateToSet}");
            // Disable this component so it doesn't repeat
            enabled = false;
        }
    }
}
