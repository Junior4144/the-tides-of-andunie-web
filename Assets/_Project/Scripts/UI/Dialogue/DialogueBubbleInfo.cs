using UnityEngine;

[CreateAssetMenu(fileName = "DialogueBubbleInfo", menuName = "Scriptable Objects/Dialogue/DialogueBubble Attributes")]
public class DialogueBubbleInfo : ScriptableObject
{
    const float defaultDuration = 3f;
    [TextArea(2, 3)]
    public string text;
    public float duration = defaultDuration;
    public float fontSize = 0f;
}
