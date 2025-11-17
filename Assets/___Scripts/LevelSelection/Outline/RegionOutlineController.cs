using UnityEngine;

public class RegionOutlineController : MonoBehaviour
{
    private LineRenderer Outline;
    private OnClickOutline onClickOutline;

    private Camera cam;

    [Header("Line Width Settings")]
    public float minCamSize = 200f;
    public float maxCamSize = 450f;
    public float minLineWidth = 1f;
    public float maxLineWidth = 10f;

    private void Awake()
    {
        Outline = GetComponentInChildren<LineRenderer>();
        onClickOutline = GetComponentInChildren<OnClickOutline>();
    }
    private void Start()
    {
        cam = CameraManager.Instance.GetCamera();
    }
    private void OnEnable()
    {
        RegionZoomController.ZoomAboveThreshold += ZoomAboveThreshold;
        RegionZoomController.ZoomBelowThreshold += ZoomBelowThreshold;
    }

    private void OnDisable()
    {
        RegionZoomController.ZoomAboveThreshold -= ZoomAboveThreshold;
        RegionZoomController.ZoomBelowThreshold -= ZoomBelowThreshold;
    }

    private void Update()
    {
        UpdateLineWidth();
    }

    private void UpdateLineWidth()
    {
        if (cam == null || Outline == null) return;

        float camSize = cam.orthographicSize;

        // Normalize camera size into 0–1 range
        float t = Mathf.InverseLerp(minCamSize, maxCamSize, camSize);

        // Lerp line width between your values
        float width = Mathf.Lerp(minLineWidth, maxLineWidth, t);

        // Apply to LineRenderer
        Outline.startWidth = width;
        Outline.endWidth = width;
    }

    private void ZoomBelowThreshold()
    {
        Outline.enabled = true;
        onClickOutline.ResetColor();
        onClickOutline.enabled = false;
    }

    private void ZoomAboveThreshold()
    {
        Outline.enabled = true;
        onClickOutline.enabled = true;
        
    }
}
