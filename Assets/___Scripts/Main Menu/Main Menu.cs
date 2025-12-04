using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MainMenu : MonoBehaviour
{
    private bool isStarting = false;
    public GameObject optionsPanel;
    public GameObject MainPanel;
    public GameObject titlePanel;
    public GameObject creditsPanel;

    [SerializeField] private float panelSwitchDelay = 0.05f;

    public void Play()
    {
        if (isStarting) return;

        Debug.Log("Clicked Play");
        isStarting = true;
        SaveGameManager.Instance.DeleteSaveFile();
        AudioManager.Instance.FadeAudio();
        SceneControllerManager.Instance.LoadNextStage(SceneManager.GetActiveScene().name, "Level0Cutscene");
    }

    public void OpenOptions()
    {
        StartCoroutine(SwitchPanels(MainPanel, optionsPanel));
    }

    public void CloseOptions()
    {
        StartCoroutine(SwitchPanels(optionsPanel, MainPanel));
    }

    public void OpenCredits()
    {
        StartCoroutine(OpenCreditsCoroutine());
    }

    public void CloseCredits()
    {
        StartCoroutine(CloseCreditsCoroutine());
    }

    private IEnumerator OpenCreditsCoroutine()
    {
        yield return new WaitForSeconds(panelSwitchDelay);

        MainPanel.SetActive(false);
        titlePanel.SetActive(false);
        creditsPanel.SetActive(true);
    }

    private IEnumerator CloseCreditsCoroutine()
    {
        yield return new WaitForSeconds(panelSwitchDelay);

        creditsPanel.SetActive(false);
        MainPanel.SetActive(true);
        titlePanel.SetActive(true);
    }

    public void HandleLoadSave()
    {
        if (!SaveGameManager.Instance.CheckSaveFile()) return;
        if (isStarting) return;

        Debug.Log("Clicked Play");
        isStarting = true;

        AudioManager.Instance.FadeAudio();

        SaveGameManager.Instance.LoadGame();

        string lastScene = SaveGameManager.Instance.data.lastScene;
        SceneControllerManager.Instance.LoadNextStage(SceneManager.GetActiveScene().name, lastScene);
    }

    private IEnumerator SwitchPanels(GameObject panelToDeactivate, GameObject panelToActivate)
    {
        yield return new WaitForSeconds(panelSwitchDelay);

        panelToDeactivate.SetActive(false);
        panelToActivate.SetActive(true);
    }

    public void Exit() =>
        Application.Quit();
}
