using UnityEngine;
using TMPro;
using System.Collections;

public class TMPFader : MonoBehaviour
{
    [SerializeField] private float fadeDuration = 1f;
    [SerializeField] private float waitDuration = 3f;

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
        yield return Fade(0f, 1f);
        yield return new WaitForSeconds(waitDuration);
        yield return Fade(1f, 0f);
    }

    private IEnumerator Fade(float startAlpha, float endAlpha)
    {
        float t = 0f;
        Color c = tmp.color;

        c.a = startAlpha;
        tmp.color = c;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float blend = t / fadeDuration;

            c.a = Mathf.Lerp(startAlpha, endAlpha, blend);
            tmp.color = c;

            yield return null;
        }
    }
}
