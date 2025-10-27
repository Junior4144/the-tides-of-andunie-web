using Unity.Cinemachine;
using UnityEngine;

public class CameraZoomController : MonoBehaviour
{
    [Header("Zoom")]
    [SerializeField] private float zoomSpeedSmall = 5f;
    [SerializeField] private float zoomSpeedLarge = 20f;
    [SerializeField] private float largeZoomThreshold = 100f;
    [SerializeField] private float minZoom = 3f;
    [SerializeField] private float maxZoom = 200f;

    [Header("Bounds")]
    [SerializeField] private BoxCollider2D boundary;

    private CinemachineCamera cam;
    private Bounds bounds;

    void Start()
    {
        cam = GetComponent<CinemachineCamera>();
        if (cam == null)
        {
            Debug.LogError("CameraZoomController requires a CinemachineCamera");
            enabled = false;
            return;
        }

        bounds = boundary.bounds;
    }

    void Update()
    {
        float scroll = Input.mouseScrollDelta.y;
        if (Mathf.Abs(scroll) < 0.01f) return;

        float current = cam.Lens.OrthographicSize;

        // ----- dynamic zoom speed -----
        float speed = current > largeZoomThreshold ? zoomSpeedLarge : zoomSpeedSmall;

        float target = Mathf.Clamp(current - scroll * speed, minZoom, maxZoom);

        // ----- bounds logic -----
        float camH = current;
        float camW = camH * cam.Lens.Aspect;
        Vector3 pos = cam.transform.position;

        bool touching =
            pos.x - camW <= bounds.min.x ||
            pos.x + camW >= bounds.max.x ||
            pos.y - camH <= bounds.min.y ||
            pos.y + camH >= bounds.max.y;

        if (scroll < 0f && touching)
            return;

        // ----- apply final zoom -----
        cam.Lens.OrthographicSize = target;
    }
}
