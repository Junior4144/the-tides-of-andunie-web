using UnityEngine;

[RequireComponent(typeof(Canvas))]
public class WorldSpaceCanvasCameraLink : MonoBehaviour
{
    private Canvas _canvas;

    private void Awake() => _canvas = GetComponent<Canvas>();

    private void Start()
    {
        if (CameraManager.Instance != null && CameraManager.Instance.GetCamera() != null)
            _canvas.worldCamera = CameraManager.Instance.GetCamera();
        else
            _canvas.worldCamera = Camera.main;
    }
}
