using UnityEngine;

public class DialogueNPC : NPC, ITalkable
{
    // For dialogue bubble
    private const float _defaultDuration = 3f;
    [TextArea(2, 3)]
    [SerializeField] private string _text;
    [SerializeField] private float _duration = _defaultDuration;
    [SerializeField] private float _fontSize = 0f;
    [SerializeField] private DialogueBubbleController _dialogueBubbleController;

    // For dialogue box
    //[SerializeField] private DialogueText dialogueText;
    //[SerializeField] private DialogueBoxController dialogueBoxController;

    public override void Interact()
    {
        //Talk(dialogueText);
        Talk(_text, _duration, _fontSize);
    }

    /*public void Talk(DialogueText dialogueText)
    {

    }*/

    public void Talk(string text, float duration, float fontSize)
    {
        _dialogueBubbleController.ShowBubble(text, duration, fontSize);
    }
}
