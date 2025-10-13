using UnityEngine;

public class DialogueBubbleManager : MonoBehaviour
{
    [SerializeField]
    private Camera _camera;
    
    void Start()
    {
        DialogueBubbleController.OnCreateDialogueBubble += GiveCamera;
    }

    public void GiveCamera(Canvas dialogueBubble)
    {
        dialogueBubble.worldCamera = _camera;
    }
}
