using UnityEngine;

public interface ITalkable
{
    //public void Talk(DialogueText dialogueText);
    public void Talk(string text, float duration, float fontSize);
}
