using UnityEngine;

public class PausedMenu : MonoBehaviour
{
    public GameObject pauseMenu;

    public bool isPaused;

    void Start() =>
        pauseMenu.SetActive(false);


    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                ResumeGame();

            else
                PauseGame();
        }
    }
    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }
}
