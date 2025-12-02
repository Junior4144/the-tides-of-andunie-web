using UnityEngine;
using UnityEngine.SceneManagement;

public class PausedMenuController : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject optionPanel;
    public bool isPaused;
    public AudioClip clickSound;

    private bool isOptionPanelActive = false;

    void Start() =>
        pauseMenu.SetActive(false);

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !isOptionPanelActive)
        {
            UIEvents.OnRequestPauseToggle?.Invoke();
        }
            
    }

    private void OnEnable()
    {
        UIEvents.OnPauseMenuDeactivated += HandlePauseMenuDeactivation;
        UIEvents.OnPauseMenuActive += HandleActivatonOfPauseMenu;
    }
    private void OnDisable()
    {
        UIEvents.OnPauseMenuDeactivated -= HandlePauseMenuDeactivation;
        UIEvents.OnPauseMenuActive -= HandleActivatonOfPauseMenu;
    }

    public void ClickResumeButton()
    {
        UIEvents.OnPauseMenuDeactivated?.Invoke();
    }

    public void HandleActivatonOfPauseMenu()
    {
        Debug.Log("PausedMenuController pausing game");
        PauseGame();
    }

    private void HandlePauseMenuDeactivation()
    {
        Debug.Log("PausedMenuController Resuming game");
        ResumeGame();
    }

    private void PauseGame()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
    }

    private void ResumeGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }

    public void HandleOptions()
    {
        Debug.Log("OPTIONS pressed � opening options menu");
        isOptionPanelActive = true;
        pauseMenu.SetActive(false);
        optionPanel.SetActive(true);
    }

    public void OptionsToPauseMenu()
    {
        Debug.Log("BACK pressed � returning to pause menu");
        isOptionPanelActive = false;
        optionPanel.SetActive(false);
        pauseMenu.SetActive(true);
    }
    public void HandleSkip()
    {
        Debug.Log("SKIP pressed � handling all transitions");

        GameObject obj = GameObject.FindGameObjectWithTag("StageEnd");
        if (obj.TryGetComponent(out SceneChangeController ecs))
            ecs.NextStage();

        if(PlayerManager.Instance)
            PlayerManager.Instance.HandleDestroy();

        UIEvents.OnRequestPauseToggle?.Invoke();
    }

    public void HandleQuit() => Application.Quit();

    public void PlayClickSound() =>
        AudioManager.Instance?.PlayOneShot(clickSound, volumeScale: 0.6f);

    public void HandleMainMenuClick()
    {
        GoToMainMenu();
    }

    public void GoToMainMenu()
    {
        AudioManager.Instance.FadeAudio();
        SaveManager.Instance.SavePlayerStats();
        PlayerManager.Instance.HandleDestroy();

        LoadNextStage();
        UIEvents.OnPauseMenuDeactivated?.Invoke();
    }

    private void LoadNextStage()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        SceneControllerManager.Instance.LoadNextStage(currentScene, "Main Menu");
    }
}
