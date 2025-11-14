using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LSTutorialTextController : MonoBehaviour
{
    [SerializeField] GameObject Panel;
    [Header("Timings")]
    [SerializeField] private float fadeDuration = 1f;
    [SerializeField] private float waitDuration = 2f;

    [Header("Tutorial Steps")]
    [SerializeField] private List<string> tutorialLines = new List<string>();

    private TMP_Text tmp;

    private void Awake()
    {
        Panel.SetActive(true);
        tmp = GetComponentInChildren<TMP_Text>();
        if (tmp == null)
            Debug.LogError("No TMP_Text found in children!", this);
    }

    private void Start()
    {
        if (GlobalStoryManager.Instance.enterLevelSelectorFirstTime == true)
        {
            Panel.SetActive(false);
            return;
        }

        GlobalStoryManager.Instance.enterLevelSelectorFirstTime = true;

        StartCoroutine(PlayTutorial());
    }

    private IEnumerator PlayTutorial()
    {
        foreach (string line in tutorialLines)
        {
            tmp.text = line;

            yield return Fade(0f, 1f);                // fade in
            yield return new WaitForSeconds(waitDuration);
            yield return Fade(1f, 0f);                // fade out
        }
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

