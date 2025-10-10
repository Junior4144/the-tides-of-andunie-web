using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class CutSceneNextScene : MonoBehaviour
{
    public float changeTime;      // How long the cutscene lasts
    public string sceneName;      // The next scene to load

    private float timer;
    private bool hasTransitioned = false;

    void Start()
    {
        timer = changeTime;
    }

    void Update()
    {
        timer -= Time.deltaTime;

        // When the cutscene ends, transition to the next scene
        if (!hasTransitioned && timer <= 0f)
        {
            hasTransitioned = true;
            StartCoroutine(LoadNextScene());
        }
    }

    IEnumerator LoadNextScene()
    {
        // Load the persistent gameplay scene first
        SceneManager.LoadScene("PersistentGameplay");

        // Load the next scene additively
        SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);

        // Unload the current cutscene scene
        yield return SceneManager.UnloadSceneAsync(gameObject.scene);
    }
}
