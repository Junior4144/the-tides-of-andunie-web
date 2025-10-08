using UnityEngine;
using UnityEngine.SceneManagement;


public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private SceneController _sceneController;
    public void play() =>
        _sceneController.LoadScene("Level0Cutscene");

    public void Exit() =>
        Application.Quit();
}
