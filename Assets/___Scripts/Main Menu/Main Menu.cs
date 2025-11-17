using UnityEngine;
using UnityEngine.SceneManagement;


public class MainMenu : MonoBehaviour
{
    private bool isStarting = false;
    public GameObject optionsPanel;
    public GameObject MainPanel;

    public void Play()
    {
        if (isStarting) return;

        Debug.Log("Clicked Play");
        isStarting = true;

        AudioManager.Instance.FadeAudio();
        SceneControllerManager.Instance.LoadNextStage("Main Menu", "Level0Cutscene");
    }

    public void OpenOptions()
    {
        optionsPanel.SetActive(true);
        MainPanel.SetActive(false);

    }

    public void CloseOptions()
    {
        optionsPanel.SetActive(false);
        MainPanel.SetActive(true);
    }

    public void Exit() =>
        Application.Quit();
}
