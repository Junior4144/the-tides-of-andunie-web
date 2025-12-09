using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SceneFade : MonoBehaviour
{
    private Image _sceneFadeImage;

    private void Awake()
    {
        _sceneFadeImage = GetComponent<Image>();

    }

    public IEnumerator FadeInCoroutine(float duration)
    {
        Color startColor = new Color(_sceneFadeImage.color.r, _sceneFadeImage.color.g, _sceneFadeImage.color.b, 1);//alpha value = 1
        Color targetColor = new Color(_sceneFadeImage.color.r, _sceneFadeImage.color.g, _sceneFadeImage.color.b, 0);
        yield return FadeCorotine(startColor, targetColor, duration);

        gameObject.SetActive(false);
    }

    public IEnumerator FadeOutCoroutine(float duration)
    {
        Color startColor = new Color(_sceneFadeImage.color.r, _sceneFadeImage.color.g, _sceneFadeImage.color.b,0);
        Color targetColor = new Color(_sceneFadeImage.color.r, _sceneFadeImage.color.g, _sceneFadeImage.color.b, 1);

        gameObject.SetActive(true);
        yield return FadeCorotine(startColor, targetColor, duration);
    }

    private IEnumerator FadeCorotine(Color startColor, Color targetColor, float duration)
    {
        float elapsedTime = 0;
        float elaspedPercentage = 0;

        //change very frame
        while(elaspedPercentage < 1)
        {
            elaspedPercentage = elapsedTime / duration;
            _sceneFadeImage.color = Color.Lerp(startColor, targetColor, elaspedPercentage);
            yield return null; //waits for next frame
            elapsedTime += Time.deltaTime;
        }
    }
}

