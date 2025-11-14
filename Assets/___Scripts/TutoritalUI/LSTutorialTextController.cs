using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LSTutorialTextController : MonoBehaviour
{
    [Header("Panels (CanvasGroups)")]
    [SerializeField] private List<CanvasGroup> panels = new List<CanvasGroup>();

    [Header("Activate These Objects on Awake")]
    [SerializeField] private List<GameObject> objectsToActivate = new List<GameObject>();

    [Header("Timings")]
    [SerializeField] private float fadeDuration = 1f;
    [SerializeField] private float waitDuration = 2f;

    private void Awake()
    {
        foreach (GameObject go in objectsToActivate)
        {
            if (go != null)
                go.SetActive(true);
        }

        // Make all panels transparent immediately at startup
        foreach (CanvasGroup cg in panels)
        {
            if (cg != null)
                cg.alpha = 0f;
        }
    }

    private void Start()
    {
        if (GlobalStoryManager.Instance.enterLevelSelectorFirstTime)
        {
            foreach (CanvasGroup cg in panels)
                cg.alpha = 0f;
            return;
        }

        GlobalStoryManager.Instance.enterLevelSelectorFirstTime = true;

        StartCoroutine(PlayTutorial());
    }

    private IEnumerator PlayTutorial()
    {
        // fade each panel one after another
        foreach (CanvasGroup panel in panels)
        {
            // ensure it starts invisible
            panel.alpha = 0f;

            // fade in
            yield return Fade(panel, 0f, 1f);

            // wait while fully visible
            yield return new WaitForSeconds(waitDuration);

            // fade out
            yield return Fade(panel, 1f, 0f);
        }
    }

    private IEnumerator Fade(CanvasGroup cg, float start, float end)
    {
        float t = 0f;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float blend = t / fadeDuration;

            cg.alpha = Mathf.Lerp(start, end, blend);

            yield return null;
        }
    }
}
