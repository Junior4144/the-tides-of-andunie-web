using System.Collections;
using UnityEngine;
using UnityEngine.UI;



public class SceneFade : MonoBehaviour
{
    private Image _sceneFadeImage;

    private void Awake() =>
        _sceneFadeImage = GetComponent<Image>();

    public IEnumerator FadeInCoroutine(float duration)
    {

        (Color startColor, Color targetColor) = ObtainColor(1f, 0f);

        yield return FadeCorotine(startColor, targetColor, duration);

        gameObject.SetActive(false);
    }

    public IEnumerator FadeOutCoroutine(float duration)
    {
        (Color startColor, Color targetColor) = ObtainColor(0f, 1f);

        gameObject.SetActive(true);

        yield return FadeCorotine(startColor, targetColor, duration);
    }

    private IEnumerator FadeCorotine(Color startColor, Color targetColor, float duration)
    {
        float elapsedTime = 0;
        float elaspedPercentage = 0;

        while (elaspedPercentage < 1)
        {
            elaspedPercentage = elapsedTime / duration;
            _sceneFadeImage.color = Color.Lerp(startColor, targetColor, elaspedPercentage);
            yield return null; 
            elapsedTime += Time.deltaTime;
        }
    }
    (Color, Color) ObtainColor(float startIndex, float targetIndex)
    {
        Color startColor = new Color(_sceneFadeImage.color.r, _sceneFadeImage.color.g, _sceneFadeImage.color.b, startIndex);
        Color targetColor = new Color(_sceneFadeImage.color.r, _sceneFadeImage.color.g, _sceneFadeImage.color.b, targetIndex);
        return (startColor, targetColor);
    }
}



