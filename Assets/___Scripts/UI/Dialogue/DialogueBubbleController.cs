using System.Collections;
using NUnit.Framework.Internal;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DialogueBubbleController : MonoBehaviour
{
    const float defaultDuration = 3f;
    const float typeSpeed = 30f;
    const string htmlAlpha = "<color=#00000000>";
    [SerializeField] private GameObject _bubblePrefab;
    [SerializeField] private float _offsetX = 0f;
    [SerializeField] private float _offsetY = 3f;
    [SerializeField] private Camera _camera;

    private GameObject _bubbleInstance;
    private TextMeshProUGUI _textMesh;
    private Coroutine _typeDialogueCoroutine;
    private bool _isShowingBubble = false;


    private void Start()
    {
        _bubbleInstance = Instantiate(_bubblePrefab, GetNewBubblePosition(), Quaternion.identity);
        _bubbleInstance.SetActive(false);

        Canvas canvas = _bubbleInstance.GetComponentInChildren<Canvas>();
        if (canvas != null && _camera != null)
            canvas.worldCamera = _camera;
        else
            Debug.LogWarning("Camera was not attached to dialogue bubble");

        _textMesh = _bubbleInstance.GetComponentInChildren<TextMeshProUGUI>();

        SceneStateManager.OnNonPersistentSceneActivated += HandleSceneLocationChange;
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

        if (_isShowingBubble)
        {
            StopCoroutine(_typeDialogueCoroutine);
            _bubbleInstance.SetActive(false);
            _isShowingBubble = false;
        }
        _textMesh.enableAutoSizing = fontSize <= 0f;
        if (fontSize > 0f)
            _textMesh.fontSize = fontSize;

        /*if (_hideCoroutine != null)
            StopCoroutine(_hideCoroutine);*/

        _typeDialogueCoroutine = StartCoroutine(TypeDialogueText(text, duration));
    }

    private IEnumerator TypeDialogueText(string text, float secondsAfterTyping)
    {
        _isShowingBubble = true;
        _bubbleInstance.SetActive(true);
        _textMesh.text = "";

        string originalText = text;
        string displayedText = "";

        for (int alphaIndex = 0; alphaIndex <= text.ToCharArray().Length; alphaIndex++)
        {
            _textMesh.text = originalText;
            displayedText = _textMesh.text.Insert(alphaIndex, htmlAlpha);
            _textMesh.text = displayedText;

            yield return new WaitForSeconds(1 / typeSpeed);
        }

        yield return new WaitForSeconds(secondsAfterTyping);
        _bubbleInstance.SetActive(false);
        _isShowingBubble = false;
    }

    private void OnDestroy()
    {
        SceneStateManager.OnNonPersistentSceneActivated -= HandleSceneLocationChange;
    }
    private void HandleSceneLocationChange() => SceneManager.MoveGameObjectToScene(_bubbleInstance, SceneManager.GetActiveScene());
}
