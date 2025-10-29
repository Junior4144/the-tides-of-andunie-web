using UnityEngine;

[RequireComponent(typeof(Canvas))]
public class WorldSpaceCanvasCameraLink : MonoBehaviour
{
    private Canvas _canvas;

    private void Awake()
    {
        _canvas = GetComponent<Canvas>();
    }

    private void Start()
    {
        // Try to grab camera from CameraManager
        if (CameraManager.Instance != null && CameraManager.Instance.GetCamera() != null)
        {
            _canvas.worldCamera = CameraManager.Instance.GetCamera();
            Debug.Log($"[WorldSpaceCanvasCameraLink] Assigned camera '{_canvas.worldCamera.name}' to Canvas '{gameObject.name}'.");
        }
        else
        {
            // Fallback: try main camera
            _canvas.worldCamera = Camera.main;
            if (_canvas.worldCamera != null)
                Debug.Log($"[WorldSpaceCanvasCameraLink] Used Camera.main for '{gameObject.name}'.");
            else
                Debug.LogWarning($"[WorldSpaceCanvasCameraLink] No camera found for Canvas '{gameObject.name}'!");
        }
    }
}
