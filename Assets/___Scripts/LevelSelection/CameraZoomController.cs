using Unity.Cinemachine;
using UnityEngine;

public class CameraZoomController : MonoBehaviour
{
    [Header("Zoom")]
    [SerializeField] private float zoomSpeed = 5f;
    [SerializeField] private float minZoom = 3f;
    [SerializeField] private float maxZoom = 12f;

    [Header("References")]
    private CinemachineCamera cam;

    void Start()
    {
        cam = GetComponent<CinemachineCamera>();
        if (cam == null) enabled = false;
    }

    void Update()
    {
        float scroll = Input.mouseScrollDelta.y;
        if (Mathf.Abs(scroll) < 0.01f) return;

        float current = cam.Lens.OrthographicSize;
        float target = Mathf.Clamp(current - scroll * zoomSpeed, minZoom, maxZoom);
        cam.Lens.OrthographicSize = target;
    }


}
