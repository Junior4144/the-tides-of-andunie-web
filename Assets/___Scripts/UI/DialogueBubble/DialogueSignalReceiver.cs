using UnityEngine;

public class DialogueSignalReceiver : MonoBehaviour
{
    private DialogueBubbleController _dialogueController;

    void Start()
    {
        StartCoroutine(FindDialogueController());
    }
    
    private System.Collections.IEnumerator FindDialogueController()
    {
        yield return null;
        
        _dialogueController = FindFirstObjectByType<DialogueBubbleController>();
        if (_dialogueController == null)
        {
            Debug.LogError("Could not find DialogueBubbleController!");
        }
    }
    
    public void ShowDialogue(string text)
    {
        _dialogueController.DialogueBubbleEventWrapper(text);
    }
    
    public void ShowDialogueLong(string text)
    {
        _dialogueController.DialogueBubbleEventWrapperLong(text);
    }
    
    public void ShowDialogueCustom(string text)
    {
        _dialogueController.DialogueBubbleEventWrapperCustom(text);
    }
}