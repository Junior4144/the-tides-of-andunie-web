using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
enum MouseButton { Left = 0, Right = 1, Middle = 2 }

public class CameraLSController : MonoBehaviour
{
    [SerializeField] private float dragSpeed = 5f;
    [SerializeField] private float dragLerpFactor = 0.02f;
    [SerializeField] private BoxCollider2D boundary;

    [Header("Cursor Sprites")]
    public Texture2D defaultCursor;  
    // same as valid for now
    public Texture2D validClickCursor;
    public Texture2D invalidClickCursor;

    public Texture2D dragCursor;
    public Vector2 hotspot = Vector2.zero;

    Camera cam;
    Vector3 lastMousePos;
    Transform PlayerTransform;
    Bounds bounds;

    private bool isAboveThreshold = false;

    public static event Action<Vector2> LockedRegionClicked;

    private void OnEnable()
    {
        RegionZoomController.ZoomAboveThreshold += OnZoomAboveThreshold;
        RegionZoomController.ZoomBelowThreshold += OnZoomBelowThreshold;

        OnClickOutline.RegionClicked += HandleRegionClicked;
    }

    private void OnDisable()
    {
        RegionZoomController.ZoomAboveThreshold -= OnZoomAboveThreshold;
        RegionZoomController.ZoomBelowThreshold -= OnZoomBelowThreshold;

        OnClickOutline.RegionClicked -= HandleRegionClicked;
    }

    private void OnZoomAboveThreshold()
    {
        isAboveThreshold = true;
    }

    private void OnZoomBelowThreshold()
    {
        isAboveThreshold = false;
    }

    void Start()
    {
        PlayerTransform = PlayerManager.Instance.GetPlayerTransform();
        cam = CameraManager.Instance.GetCamera();
        bounds = boundary.bounds;

        transform.position = new Vector3(PlayerTransform.position.x, PlayerTransform.position.y, transform.position.z);
        Cursor.SetCursor(defaultCursor, hotspot, CursorMode.Auto);
    }

    void Update()
    {
        HandleDrag(MouseButton.Right);
        HandleDrag(MouseButton.Middle);
        HandleLeftClick();
    }


    void HandleDrag(MouseButton button)
    {
        if (Input.GetMouseButtonDown((int)button))
        {
            lastMousePos = cam.ScreenToWorldPoint(Input.mousePosition);
            Cursor.SetCursor(dragCursor, hotspot, CursorMode.Auto);
        }

        if (Input.GetMouseButton((int)button))
        {
            Vector3 currentMousePos = cam.ScreenToWorldPoint(Input.mousePosition);
            Vector3 diff = lastMousePos - currentMousePos;

            Vector3 newPos = transform.position + diff * dragSpeed;
            float camHeight = cam.orthographicSize;
            float camWidth = camHeight * cam.aspect;

            newPos.x = Mathf.Clamp(newPos.x,
                bounds.min.x + camWidth,
                bounds.max.x - camWidth);

            newPos.y = Mathf.Clamp(newPos.y,
                bounds.min.y + camHeight,
                bounds.max.y - camHeight);

            transform.position = Vector3.Lerp(transform.position, newPos, dragLerpFactor);
        }

        if (Input.GetMouseButtonUp((int)button))
        {
            Cursor.SetCursor(defaultCursor, hotspot, CursorMode.Auto);
        }
    }

    void HandleLeftClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mouseWorld = cam.ScreenToWorldPoint(Input.mousePosition);
            HandleBelowThresholdClick(mouseWorld);
        }

        if (Input.GetMouseButtonUp(0))
        {
            Cursor.SetCursor(defaultCursor, hotspot, CursorMode.Auto);
        }
    }

    void HandleRegionClicked(Region region)
    {
        Cursor.SetCursor(validClickCursor, hotspot, CursorMode.Auto);
    }

    void HandleBelowThresholdClick(Vector2 mouseWorld)
    {
        bool isVillage = false;

        // ---------------------------------
        // 1. CHECK VILLAGE POINTER TARGET
        // ---------------------------------
        Collider2D[] hits = Physics2D.OverlapPointAll(mouseWorld);

        foreach (var h in hits)
        {
            if (h.CompareTag("LSVillagePointerTarget"))
            {
                isVillage = true;
                break;
            }
        }

        // ---------------------------------
        // 2. IF NOT A VILLAGE → CHECK NAVMESH
        // ---------------------------------
        bool navValid = false;

        if (!isVillage)
            navValid = NavMesh.SamplePosition(mouseWorld, out _, 0.3f, NavMesh.AllAreas);
        else
            navValid = true; // Village acts like a valid nav target

        if (!navValid)
        {
            Cursor.SetCursor(invalidClickCursor, hotspot, CursorMode.Auto);
            return;
        }

        // ---------------------------------
        // 3. REGION CHECK (FINAL DECISION MAKER)
        // ---------------------------------
        bool regionLocked = false;

        foreach (var h in hits)
        {
            if (h.TryGetComponent<RegionInfo>(out var region))
            {
                if (LSRegionLockManager.Instance.IsRegionLocked(region))
                {
                    regionLocked = true;
                    LockedRegionClicked?.Invoke(mouseWorld);
                    break;
                }
            }
        }

        // ---------------------------------
        // 4. FINAL RESULT BASED ON REGION LOCK
        // ---------------------------------
        if (regionLocked)
        {
            Cursor.SetCursor(invalidClickCursor, hotspot, CursorMode.Auto);
            return;
        }

        // ---------------------------------
        // 5. VALID RESULT
        // ---------------------------------
        if (isVillage)
            Debug.Log("Hit the correct Village Pointer Target!");

        Cursor.SetCursor(validClickCursor, hotspot, CursorMode.Auto);
    }
}
