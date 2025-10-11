using UnityEngine;
using UnityEngine.SceneManagement;


public class MainMenu : MonoBehaviour
{

    public void play()
    {
        Debug.Log("Clicked Play");
        SceneControllerManager.Instance.LoadNextStage("Main Menu", "Level0Cutscene");

    }
        
    public void Exit() =>
        Application.Quit();
}
