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

    [Header("Line Transparency Settings")]
    public float minAlpha = 0.1f;
    public float maxAlpha = 1f;

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
        Outline.startWidth = width;
        Outline.endWidth = width;

        // --- UPDATED ALPHA LOGIC ---
        float alpha;

        // If camera size is below the threshold → keep alpha fixed at 0.1
        if (camSize <= minCamSize)
        {
            alpha = minAlpha;
        }
        else
        {
            // Remap camera size only for values ABOVE 200
            float tAlpha = Mathf.InverseLerp(minCamSize, maxCamSize, camSize);

            alpha = Mathf.Lerp(minAlpha, maxAlpha, tAlpha);
        }

        // Apply alpha
        Color c = Outline.startColor;
        c.a = alpha;
        Outline.startColor = c;
        Outline.endColor = c;
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
