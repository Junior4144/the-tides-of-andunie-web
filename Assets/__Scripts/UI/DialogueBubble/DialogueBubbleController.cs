using UnityEngine;
using TMPro;
using System;

public class DialogueBubbleController : MonoBehaviour
{
    const int numBubbleParameters = 3;
    const float defaultDuration = 3f;
    [SerializeField]
    private GameObject _bubblePrefab;
    [SerializeField]
    private float _offsetX = 0f;
    [SerializeField]
    private float _offsetY = 3f;
    private GameObject _currentBubble = null;

    public static event Action<Canvas> OnCreateDialogueBubble;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PlayerHealthController.OnHealthGained += MakeDialogueBubble;
    }

    void OnEnabled()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (_currentBubble != null)
        {
            FollowUnit();
        }
    }

    public void DialogueBubbleEventWrapper(string text)
    {
        MakeDialogueBubble(text, defaultDuration, 0f);
    }

    public void DialogueBubbleEventWrapperLong(string text)
    {
        MakeDialogueBubble(text, 10f, 0f);
    }

    public void DialogueBubbleEventWrapperCustom(string text)
    {
        MakeDialogueBubble(text, 5f, 25f);
    }

    // etc

    // Or
    public void DialogueBubbleEventWrapperParsing(string textDurationFontSize)
    {
        string[] info = "".Split(' ');

        // Parsing
        // info = parsed values

        if (info.Length != numBubbleParameters)
        {
            Debug.LogError($"String should have {numBubbleParameters} elements");
            return;
        }

        FormatInput(info);

        string text = info[0];
        float duration = float.Parse(info[1]);
        float fontSize = float.Parse(info[2]);
        MakeDialogueBubble(text, duration, fontSize);
    }

    private void FollowUnit()
    {
        _currentBubble.transform.position = GetNewBubblePosition();
    }

    private void FormatInput(string[] info)
    {
        float duration, fontSize;

        try
        {
            duration = float.Parse(info[1]);
        }
        catch (Exception e)
        {
            Debug.LogWarning(e);
            duration = defaultDuration;
        }
        info[1] = duration.ToString();

        try
        {
            fontSize = float.Parse(info[2]);
        }
        catch (Exception e)
        {
            Debug.LogWarning(e);
            fontSize = 0f;
        }
        info[2] = fontSize.ToString();
    }

    private Vector3 GetNewBubblePosition()
    {
        float bubbleX = transform.position.x + _offsetX;
        float bubbleY = transform.position.y + _offsetY;
        return new Vector3(bubbleX, bubbleY, 0f);
    }

    private void MakeDialogueBubble(string text, float duration = 3f, float fontSize = 0f)
    {
        if (_currentBubble != null)
        {
            Destroy(_currentBubble.gameObject);
        }

        _currentBubble = Instantiate(_bubblePrefab, GetNewBubblePosition(), Quaternion.identity);
        if (_currentBubble)
        {
            OnCreateDialogueBubble?.Invoke(_currentBubble.GetComponentInChildren<Canvas>());
        }
        SetBubbleText(text, fontSize);

        Destroy(_currentBubble.gameObject, duration);
    }

    private void SetBubbleText(string text, float fontSize)
    {
        TextMeshProUGUI textMesh = _currentBubble.GetComponentInChildren<TextMeshProUGUI>();
        if (textMesh == null)
        {
            Debug.Log($"TextMesh with intended text: '{text}' is null");
        }
        if (fontSize > 0f)
        {
            textMesh.enableAutoSizing = false;
            textMesh.fontSize = fontSize;
        }
        textMesh.text = text;
    }
}
