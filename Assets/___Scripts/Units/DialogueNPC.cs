using UnityEngine;

public class DialogueNPC : NPC, ITalkable
{
    // For dialogue bubble
    [SerializeField] private DialogueBubbleInfo _dialogueBubbleInfo;
    [SerializeField] private DialogueBubbleController _dialogueBubbleController;

    // For dialogue box
    //[SerializeField] private DialogueText dialogueText;
    //[SerializeField] private DialogueBoxController dialogueBoxController;

    public override void Interact()
    {
        //Talk(dialogueText);
        Talk(_dialogueBubbleInfo);
    }

    public void Talk(DialogueText dialogueText)
    {

    }

    public void Talk(DialogueBubbleInfo dialogueBubbleInfo)
    {
        _dialogueBubbleController.ShowBubble(dialogueBubbleInfo.text, dialogueBubbleInfo.duration, dialogueBubbleInfo.fontSize);
    }
}
