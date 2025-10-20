using UnityEngine;

public interface ITalkable
{
    public void Talk(DialogueText dialogueText);
    public void Talk(DialogueBubbleInfo dialogueBubbleInfo);
}
