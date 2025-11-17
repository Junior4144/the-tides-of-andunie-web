using UnityEngine;

public class RegionOutlineController : MonoBehaviour
{
    private LineRenderer Outline;
    private OnClickOutline onClickOutline;

    private void Awake()
    {
        Outline = GetComponentInChildren<LineRenderer>();
        onClickOutline = GetComponentInChildren<OnClickOutline>();
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

    private void ZoomBelowThreshold()
    {
        Outline.enabled = false;
        onClickOutline.enabled = false;
    }

    private void ZoomAboveThreshold()
    {
        Outline.enabled = true;
        onClickOutline.enabled = true;
        
    }

}
