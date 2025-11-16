using Unity.Cinemachine;
using UnityEngine;

public class CameraZoomController : MonoBehaviour
{
    [Header("Zoom")]
    [SerializeField] private float zoomFactor = 0.15f;      // Controls normalized zoom strength
    [SerializeField] private float minZoom = 10f;
    [SerializeField] private float maxZoom = 400f;

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

        if (boundary != null)
            bounds = boundary.bounds;
    }

    void Update()
    {
        float current = cam.Lens.OrthographicSize;
        float scroll = Input.mouseScrollDelta.y;

        // ------------------------------------------------------------
        // MANUAL ZOOM (normalized)
        // ------------------------------------------------------------
        if (Mathf.Abs(scroll) > 0.01f)
        {
            float delta = NormalizedZoomDelta(-scroll);
            float target = Mathf.Clamp(current + delta, minZoom, maxZoom);

            if (scroll < 0 && IsTouchingBounds(target))
                return;

            cam.Lens.OrthographicSize = target;
        }
    }

    // ------------------------------------------------------------
    // NORMALIZED ZOOM DELTA (consistent zoom everywhere)
    // ------------------------------------------------------------
    float NormalizedZoomDelta(float input)
    {
        float scale = cam.Lens.OrthographicSize;
        return input * (scale * zoomFactor);
    }


    // ------------------------------------------------------------
    // BOUNDARY CHECK
    // ------------------------------------------------------------
    bool IsTouchingBounds(float ortho)
    {
        if (boundary == null) return false;

        float camH = ortho;
        float camW = camH * cam.Lens.Aspect;
        Vector3 pos = cam.transform.position;

        return
            pos.x - camW <= bounds.min.x ||
            pos.x + camW >= bounds.max.x ||
            pos.y - camH <= bounds.min.y ||
            pos.y + camH >= bounds.max.y;
    }
}
