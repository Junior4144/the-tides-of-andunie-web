using System.Collections;
using UnityEngine;

public class HealthBarShake : MonoBehaviour
{
    public float _duration = 0.3f;
    public float _magnitude = 10f;

    private RectTransform _rectTransform;
    private Vector2 _originalPosition;

    void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _originalPosition = _rectTransform.anchoredPosition;
    }

    public void Shake()
    {
        StopAllCoroutines();
        StartCoroutine(DoShake());
    }

    private IEnumerator DoShake()
    {
        float elapsed = 0f;

        while (elapsed < _duration)
        {
            _rectTransform.anchoredPosition = _originalPosition + GetRandomShakeOffset();

            elapsed += Time.deltaTime;
            yield return null;
        }

        ResetPosition();
    }

    private Vector2 GetRandomShakeOffset()
    {
        float offsetX = Random.Range(-1f, 1f) * _magnitude;
        float offsetY = Random.Range(-1f, 1f) * _magnitude;
        return new Vector2(offsetX, offsetY);
    }

    private void ResetPosition() =>
        _rectTransform.anchoredPosition = _originalPosition;
}
