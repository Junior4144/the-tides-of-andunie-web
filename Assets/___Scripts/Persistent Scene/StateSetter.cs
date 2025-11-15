using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StateSetter : MonoBehaviour
{
    [SerializeField]
    private GameState stateToSet;

    void Update()
    {
        Scene activeScene = SceneManager.GetActiveScene();
        Debug.Log("[State Setter]");
        if (gameObject.scene == activeScene)
        {
            if (GameManager.Instance != null)
                GameManager.Instance.SetState(stateToSet);

            Debug.Log($"[State Setter] {activeScene.name} is active and  now SetState to {stateToSet}");
            enabled = false;
        }
    }
}

