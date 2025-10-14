using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartLevel : MonoBehaviour
{
    [SerializeField]
    RestartLevelSceneController _sceneController;

    public void RestartCurrentLevel()
    {
        _sceneController.LoadNextStage(SceneManager.GetActiveScene().name);
    }
}
