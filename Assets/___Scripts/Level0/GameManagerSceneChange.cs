using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerSceneChange : MonoBehaviour
{
    [SerializeField]
    private float _timeToWaitBeforeExit;

    [SerializeField]
    private SceneController _sceneController;

    public void OnPlayerDied() =>
        Invoke(nameof(EndGame), _timeToWaitBeforeExit);

    private void EndGame() =>
        _sceneController.LoadScene("Level 0");
}
