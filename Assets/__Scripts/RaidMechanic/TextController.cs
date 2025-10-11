using UnityEngine;
using TMPro;
using System.Collections;

public class TextController
{
    private readonly TextMeshProUGUI _textComponent;

    public TextController(TextMeshProUGUI textComponent)
    {
        _textComponent = textComponent;
        SetTextInvisible();
    }

    public void SetTextVisible() => SetTextVisibility(1f);

    public void SetTextInvisible() => SetTextVisibility(0f);

    public void SetTextVisibility(float alpha)
    {
        Color color = _textComponent.color;
        color.a = Mathf.Clamp01(alpha);
        _textComponent.color = color;
    }

    public bool IsTextVisible() => _textComponent.color.a > 0f;
    public void SetText(string text)
    {
        _textComponent.text = text;
    }

    public IEnumerator FadeOut(float fadeDuration)
    {
        float timer = 0f;
        Color startColor = _textComponent.color;
        Color targetColor = startColor;
        targetColor.a = 0f;

        while (timer < fadeDuration)
        {
            float t = timer / fadeDuration;
            _textComponent.color = Color.Lerp(startColor, targetColor, t);

            timer += Time.deltaTime;
            yield return null;
        }

        _textComponent.color = targetColor;
    }
}