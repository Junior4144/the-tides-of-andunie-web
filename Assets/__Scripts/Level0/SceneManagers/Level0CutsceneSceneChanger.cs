
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Level0CutsceneSceneChanger : MonoBehaviour
{
    public float changeTime;
    public string sceneName;
    
    [Header("Loading Settings")]
    [Tooltip("Start loading this many seconds before the transition")]
    public float preloadTime = 2f;
    
    [Tooltip("Optional fade canvas - assign a CanvasGroup for fade effect")]
    public CanvasGroup fadeCanvas;
    
    [Tooltip("Fade duration in seconds")]
    public float fadeDuration = 1f;

    private bool hasStartedLoading = false;
    private float timer;

    void Start()
    {
        timer = changeTime;
        
        // Make sure fade canvas starts transparent if assigned
        if (fadeCanvas != null)
        {
            fadeCanvas.alpha = 0f;
            fadeCanvas.gameObject.SetActive(true);
        }
    }

    void Update()
    {
        timer -= Time.deltaTime;

        // Start preloading the scene before the cutscene ends
        if (!hasStartedLoading && timer <= preloadTime)
        {
            hasStartedLoading = true;
            StartCoroutine(LoadSceneAsync());
        }
    }

    IEnumerator LoadSceneAsync()
    {
        // Start preloading the next scene (additive)
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        asyncLoad.allowSceneActivation = false;

        // Wait until the scene is mostly loaded (stops at 0.9f progress)
        while (asyncLoad.progress < 0.9f)
        {
            yield return null;
        }

        // Wait for the cutscene timer to finish
        while (timer > 0)
        {
            yield return null;
        }

        // Optional: Fade out before transition
        if (fadeCanvas != null)
        {
            yield return StartCoroutine(FadeOut());
        }

        asyncLoad.allowSceneActivation = true;

        // Wait until both are loaded
        while(!asyncLoad.isDone)
        {
            yield return null;
        }
        
        Scene nextScene = SceneManager.GetSceneByName(sceneName);
        SceneManager.SetActiveScene(nextScene);

        // Unload the current cutscene scene
        yield return SceneManager.UnloadSceneAsync(gameObject.scene);
    }


    IEnumerator FadeOut()
    {
        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            fadeCanvas.alpha = Mathf.Lerp(0f, 1f, elapsed / fadeDuration);
            yield return null;
        }
        fadeCanvas.alpha = 1f;
    }
}