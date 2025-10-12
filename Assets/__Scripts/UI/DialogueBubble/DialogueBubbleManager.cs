using UnityEngine;

public class DialogueBubbleManager : MonoBehaviour
{
    [SerializeField]
    private Camera _camera;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        DialogueBubbleController.OnCreateDialogueBubble += GiveCamera;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GiveCamera(Canvas dialogueBubble)
    {
        dialogueBubble.worldCamera = _camera;
    }
}
