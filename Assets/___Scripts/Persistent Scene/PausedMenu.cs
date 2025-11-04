using UnityEngine;

public class PausedMenu : MonoBehaviour
{
    public GameObject pauseMenu;

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
    }

    public void HandleSkip()
    {
        Debug.Log("SKIP pressed — handling all transitions");

        GameObject obj = GameObject.FindGameObjectWithTag("StageEnd");
        if (obj.TryGetComponent(out EndCurrentScene ecs))
            ecs.NextStage();
    }

    public void HandleQuit() => Application.Quit();

}
