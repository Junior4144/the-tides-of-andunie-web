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
        RegionZoomController.ZoomAbove100 += OnZoomAbove100;
        RegionZoomController.ZoomBelow100 += OnZoomBelow100;
    }

    private void OnDisable()
    {
        RegionZoomController.ZoomAbove100 -= OnZoomAbove100;
        RegionZoomController.ZoomBelow100 -= OnZoomBelow100;
    }

    private void OnZoomBelow100()
    {
        Outline.enabled = false;
        onClickOutline.enabled = false;
    }

    private void OnZoomAbove100()
    {
        Outline.enabled = true;
        onClickOutline.enabled = true;
    }

}
