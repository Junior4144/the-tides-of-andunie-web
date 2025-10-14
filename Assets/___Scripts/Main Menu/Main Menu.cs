using UnityEngine;
using UnityEngine.SceneManagement;


public class MainMenu : MonoBehaviour
{
    private bool isStarting = false;
    public void play()
    {
        if (isStarting) return;

        Debug.Log("Clicked Play");
        isStarting = true;
        SceneControllerManager.Instance.LoadNextStage("Main Menu", "Level0Cutscene");
    }
        
    public void Exit() =>
        Application.Quit();
}
