using UnityEngine;

public class DialogueNPC : NPC, ITalkable
{
    [SerializeField] private DialogueText dialogueTextScript;

    public override void Interact()
    {
        Talk(dialogueTextScript);
    }

    public void Talk(DialogueText dialogueText)
    {

    }
}
