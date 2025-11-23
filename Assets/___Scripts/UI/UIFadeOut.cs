using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIFadeOut : MonoBehaviour
{
    [Header("Panel Reference")]
    [SerializeField] private GameObject panel;

    [Header("Fade Settings")]
    [SerializeField] private float noShowDelay = 1f;
    [SerializeField] private float fadeInDuration = 1f;
    [SerializeField] private float stayDuration = 2f;
    [SerializeField] private float fadeOutDuration = 1f;

    private CanvasGroup canvasGroup;

    private void Awake()
    {
        if (panel == null)
        {
            Debug.LogError("Panel reference is missing in UIFadeOut.");
            return;
        }

        canvasGroup = panel.GetComponent<CanvasGroup>();

        if (canvasGroup == null)
        {
            Debug.LogError("The assigned panel does not contain a CanvasGroup component.");
            return;
        }

        canvasGroup.alpha = 0f;
    }

    private void OnEnable() => SceneManager.activeSceneChanged += HandleCheck;

    private void OnDisable() => SceneManager.activeSceneChanged -= HandleCheck;

    private void HandleCheck(Scene oldScene, Scene newScene)
    {
        StartCoroutine(CheckAfterLoading(newScene));
    }

    private IEnumerator CheckAfterLoading(Scene newScene)
    {
        yield return null;

        if (newScene == gameObject.scene)
            HandleSetup();
    }

    private void HandleSetup()
    {
        if (canvasGroup == null) return;

        panel.SetActive(true);

        StopAllCoroutines();
        StartCoroutine(FadeSequence());
    }

    private IEnumerator FadeSequence()
    {
        yield return StartCoroutine(FadeIn());
        yield return StartCoroutine(StayVisible());
        yield return StartCoroutine(FadeOut());

        panel.SetActive(false);
    }

    private IEnumerator FadeIn()
    {
        // 1. Keep panel hidden for a while
        canvasGroup.alpha = 0f;
        yield return new WaitForSeconds(noShowDelay);

        // 2. Then fade in normally
        float elapsed = 0f;

        while (elapsed < fadeInDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / fadeInDuration);
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, t);
            yield return null;
        }

        canvasGroup.alpha = 1f;
    }

    private IEnumerator StayVisible()
    {
        yield return new WaitForSeconds(stayDuration);
    }

    private IEnumerator FadeOut()
    {
        float elapsed = 0f;
        while (elapsed < fadeOutDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsed / fadeOutDuration);
            yield return null;
        }
        canvasGroup.alpha = 0f;
    }
}