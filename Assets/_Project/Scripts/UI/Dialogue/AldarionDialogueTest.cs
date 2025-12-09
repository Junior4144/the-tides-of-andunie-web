using UnityEngine;

public class AldarionDialogueTest : MonoBehaviour, ITalkable
{
    [SerializeField] private DialogueBubbleInfo _dialogueBubbleInfo;
    private DialogueBubbleController _dialogueBubbleController;

    void Start()
    {
        _dialogueBubbleController = GetComponent<DialogueBubbleController>();
        if (_dialogueBubbleController == null )
        {
            Debug.LogError("DialogueBubbleController script not found");
            this.enabled = false;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            Talk(_dialogueBubbleInfo);
        }
    }

    public void Talk(DialogueText dialogueText)
    {}

    public void Talk(DialogueBubbleInfo dialogueBubbleInfo)
    {
        _dialogueBubbleController.ShowBubble(dialogueBubbleInfo.text, dialogueBubbleInfo.duration, dialogueBubbleInfo.fontSize);
    }
}
