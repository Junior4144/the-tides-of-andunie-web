using UnityEngine;
using UnityEngine.SceneManagement;

public class HandlePlayerDeath : MonoBehaviour
{

    public void NextStage() =>
        LoadNextStage();

    void LoadNextStage() =>
        RestartLevelSceneController.Instance.LoadNextStage(SceneManager.GetActiveScene().name);

}
