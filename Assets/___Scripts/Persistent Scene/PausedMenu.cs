using UnityEngine;

public class PausedMenu : MonoBehaviour
{
    public GameObject pauseMenu;

    public GameObject optionPanel;

    public bool isPaused;

    void Start() =>
        pauseMenu.SetActive(false);

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            HandlePause();
    }

    public void HandlePause()
    {
        Debug.Log("trying to pause game");
        UIEvents.OnRequestPauseToggle?.Invoke();
        optionPanel.SetActive(false);
    }

    public void HandleOptions()
    {
        Debug.Log("OPTIONS pressed � opening options menu");
        pauseMenu.SetActive(false);
        optionPanel.SetActive(true);
    }

    public void OptionsToPauseMenu()
    {
        Debug.Log("BACK pressed � returning to pause menu");
        optionPanel.SetActive(false);
        pauseMenu.SetActive(true);
    }
    public void HandleSkip()
    {
        Debug.Log("SKIP pressed � handling all transitions");

        GameObject obj = GameObject.FindGameObjectWithTag("StageEnd");
        if (obj.TryGetComponent(out EndCurrentScene ecs))
            ecs.NextStage();
        if(PlayerManager.Instance)
            PlayerManager.Instance.HandleDestroy();
        UIEvents.OnRequestPauseToggle?.Invoke();
    }

    public void HandleQuit() => Application.Quit();

}
