using UnityEngine;
using TMPro;
using System.Collections;

public class TMPFader : MonoBehaviour
{
    [SerializeField] private float fadeDuration = 1f;
    [SerializeField] private float waitDuration = 3f;
    [SerializeField] private CanvasGroup canvasGroup;

    private TMP_Text tmp;   // works for both UGUI + world TMP

    private void Awake()
    {
        tmp = GetComponentInChildren<TMP_Text>();

        if (tmp == null)
            Debug.LogError("TMPFader: No TMP_Text found in children!", this);
    }


    private void Start()
    {
        StartCoroutine(FadeSequence());
    }

    private IEnumerator FadeSequence()
    {
        yield return Fade(0f, 1f);                      // fade in
        yield return new WaitForSeconds(waitDuration);
        yield return Fade(1f, 0f);                      // fade out
    }

    private IEnumerator Fade(float startAlpha, float endAlpha)
    {
        float t = 0f;

        // apply to canvas group
        if (canvasGroup != null)
            canvasGroup.alpha = startAlpha;

        // apply to TMP text
        if (tmp != null)
        {
            Color c = tmp.color;
            c.a = startAlpha;
            tmp.color = c;
        }

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float blend = t / fadeDuration;

            float newAlpha = Mathf.Lerp(startAlpha, endAlpha, blend);

            // Fade the panel / UI group
            if (canvasGroup != null)
                canvasGroup.alpha = newAlpha;

            // Fade the TMP text
            if (tmp != null)
            {
                Color c = tmp.color;
                c.a = newAlpha;
                tmp.color = c;
            }

            yield return null;
        }
    }
}