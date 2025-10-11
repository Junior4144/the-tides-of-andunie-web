using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Level0CutsceneSceneChangerv2 : MonoBehaviour
{
    [Header("Scene Settings")]
    public float changeTime;          // Duration of the cutscene
    public string sceneName;          // Name of the next scene

    [Header("Fade Settings (Optional)")]
    [Tooltip("Optional fade canvas - assign a CanvasGroup for fade effect")]
    public CanvasGroup fadeCanvas;

    [Tooltip("Fade duration in seconds")]
    public float fadeDuration = 1f;

    private float timer;

    void Start()
    {
        timer = changeTime;

        if (fadeCanvas != null)
        {
            fadeCanvas.alpha = 0f;
            fadeCanvas.gameObject.SetActive(true);
        }
    }

    void Update()
    {
        timer -= Time.deltaTime;

        // When cutscene finishes, start transition
        if (timer <= 0)
        {
            StartCoroutine(LoadNextScene());
            enabled = false; // Disable Update to prevent multiple triggers
        }
    }

    IEnumerator LoadNextScene()
    {
        // Optional fade-out effect
        if (fadeCanvas != null)
        {
            yield return StartCoroutine(FadeOut());
        }

        // Load the next scene normally (single mode replaces current)
        SceneManager.LoadScene(sceneName);
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
