using UnityEngine;

[CreateAssetMenu(fileName = "DialogueBoxText", menuName = "Scriptable Objects/Dialogue/DialogueBox Attributes")]
public class DialogueText : ScriptableObject
{
    public string speakerName;

    [TextArea(5, 10)]
    public string[] paragraphs;
}
