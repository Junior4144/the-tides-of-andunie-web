using Unity.Cinemachine;
using UnityEngine;

public class CameraZoomController : MonoBehaviour
{
    [Header("Zoom")]
    [SerializeField] private float zoomSpeedSmall = 4f;
    [SerializeField] private float zoomSpeedLarge = 10f;
    [SerializeField] private float largeZoomThreshold = 100f;
    [SerializeField] private float minZoom = 10f;
    [SerializeField] private float maxZoom = 400f;

    [Header("Auto Zoom")]
    [SerializeField] private float autoZoomOutSpeed = 20f;
    [SerializeField] private float autoZoomInSpeed = 20f;
    [SerializeField] private bool autoZoomEnabled = true;
    [SerializeField] private float thresholdBuffer = 10f;

    [Header("Bounds")]
    [SerializeField] private BoxCollider2D boundary;

    private CinemachineCamera cam;
    private Bounds bounds;
    private bool isAutoZoomingOut = false;
    private bool isAutoZoomingIn = false;

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
        float current = cam.Lens.OrthographicSize;
        float scroll = Input.mouseScrollDelta.y;

        // --------------------------------------------------------------------
        // AUTO ZOOM IN MODE
        // --------------------------------------------------------------------
        if (isAutoZoomingIn)
        {
            float target = Mathf.MoveTowards(current, largeZoomThreshold, autoZoomInSpeed * Time.deltaTime);
            cam.Lens.OrthographicSize = target;

            // Stop auto zoom-in when we reach the threshold
            if (target <= largeZoomThreshold)
            {
                isAutoZoomingIn = false;
            }

            return; // user cannot zoom right now
        }

        // --------------------------------------------------------------------
        // AUTO ZOOM OUT MODE
        // --------------------------------------------------------------------
        if (isAutoZoomingOut)
        {
            float target = Mathf.MoveTowards(current, maxZoom, autoZoomOutSpeed * Time.deltaTime);
            cam.Lens.OrthographicSize = target;

            // Stop auto zoom-out when maxZoom reached
            if (Mathf.Abs(target - maxZoom) < 0.01f)
            {
                isAutoZoomingOut = false;
            }

            return; // user cannot zoom right now
        }

        // --------------------------------------------------------------------
        // START AUTO ZOOM IN (scroll inward while above threshold+buffer)
        // --------------------------------------------------------------------
        if (autoZoomEnabled && scroll > 0 && current > largeZoomThreshold + thresholdBuffer)
        {
            isAutoZoomingIn = true;
            return;
        }

        // --------------------------------------------------------------------
        // START AUTO ZOOM OUT (no scroll, above threshold+buffer)
        // --------------------------------------------------------------------
        if (autoZoomEnabled && Mathf.Abs(scroll) < 0.01f
            && current >= largeZoomThreshold + thresholdBuffer
            && current < maxZoom)
        {
            isAutoZoomingOut = true;
            return;
        }

        // --------------------------------------------------------------------
        // NORMAL MANUAL ZOOM (only if no auto zoom is active)
        // --------------------------------------------------------------------
        if (Mathf.Abs(scroll) > 0.01f)
        {
            float speed = current > largeZoomThreshold ? zoomSpeedLarge : zoomSpeedSmall;
            float targetManual = Mathf.Clamp(current - scroll * speed, minZoom, maxZoom);

            // Bound checks
            float camH = current;
            float camW = camH * cam.Lens.Aspect;
            Vector3 pos = cam.transform.position;

            bool touching =
                pos.x - camW <= bounds.min.x ||
                pos.x + camW >= bounds.max.x ||
                pos.y - camH <= bounds.min.y ||
                pos.y + camH >= bounds.max.y;

            if (scroll < 0 && touching)
                return;

            cam.Lens.OrthographicSize = targetManual;
        }
    }
}
