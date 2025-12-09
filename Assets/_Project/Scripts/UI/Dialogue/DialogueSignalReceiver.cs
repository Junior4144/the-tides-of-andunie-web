using UnityEngine;

public class DialogueSignalReceiver : MonoBehaviour
{
    [SerializeField] private DialogueBubbleController _dialogueController;

    void Start()
    {
        if (!_dialogueController)
        {
            _dialogueController = FindFirstObjectByType<DialogueBubbleController>();

            if (!_dialogueController)
            {
                Debug.LogError("[DialogueSignalReceiver] Could not find DialogueBubbleController!");
            }
        }
    }
    
    public void ShowDialogue(string text)
    {
        _dialogueController.ShowBubble(text, 2f);
    }
    
    public void ShowDialogueLong(string text)
    {
        _dialogueController.ShowBubble(text, 4f);
    }
}