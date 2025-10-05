using UnityEngine;
using UnityEngine.SceneManagement;


public class MainMenu : MonoBehaviour
{

    public void play() =>
        SceneManager.LoadScene("Level 0");

    public void Exit() =>
        Application.Quit();
}
