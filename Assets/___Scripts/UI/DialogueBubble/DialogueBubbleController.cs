using UnityEngine;
using TMPro;
using System.Collections;
using NUnit.Framework.Internal;
using UnityEngine.UI;

public class DialogueBubbleController : MonoBehaviour
{
    const float defaultDuration = 3f;
    [SerializeField] private GameObject _bubblePrefab;
    [SerializeField] private float _offsetX = 0f;
    [SerializeField] private float _offsetY = 3f;
    [SerializeField] private Camera _camera;

    private GameObject _bubbleInstance;
    private TextMeshProUGUI _textMesh;
    private Coroutine _hideCoroutine;

    private void Start()
    {
        _bubbleInstance = Instantiate(_bubblePrefab, GetNewBubblePosition(), Quaternion.identity);
        _bubbleInstance.SetActive(false);

        Canvas canvas = _bubbleInstance.GetComponentInChildren<Canvas>();
        if (canvas != null && _camera != null)
            canvas.worldCamera = _camera;

        _textMesh = _bubbleInstance.GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Update()
    {
        if (_bubbleInstance.activeSelf)
            _bubbleInstance.transform.position = GetNewBubblePosition();
    }

    private Vector3 GetNewBubblePosition() =>
        new Vector3(transform.position.x + _offsetX, transform.position.y + _offsetY, 0f);


    public void ShowBubbleFromSignal(string text)
    {
        ShowBubble(text);
    }

    public void ShowBubble(string text, float duration = defaultDuration, float fontSize = 0f)
    {
        if (_textMesh == null) return;

        _textMesh.text = text;
        _textMesh.enableAutoSizing = fontSize <= 0f;
        if (fontSize > 0f)
            _textMesh.fontSize = fontSize;

        if (_hideCoroutine != null)
            StopCoroutine(_hideCoroutine);

        _bubbleInstance.SetActive(true);
        _hideCoroutine = StartCoroutine(HideAfterSeconds(duration));
    }

    private IEnumerator HideAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        _bubbleInstance.SetActive(false);
    }
}
