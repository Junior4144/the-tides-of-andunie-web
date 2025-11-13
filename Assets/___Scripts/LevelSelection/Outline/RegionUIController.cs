using UnityEngine;

public class RegionUIController : MonoBehaviour
{
    [SerializeField] private GameObject RegionCanvas;
    [SerializeField] private GameObject RegionPanel;

    private void OnEnable()
    {
        OnClickOutline.RegionClicked += HandleRegionClicked;
        RegionZoomController.ZoomAbove100 += OnZoomAbove100;
        RegionZoomController.ZoomBelow100 += OnZoomBelow100;
    }

    private void OnDisable()
    {
        OnClickOutline.RegionClicked -= HandleRegionClicked;
        RegionZoomController.ZoomAbove100 -= OnZoomAbove100;
        RegionZoomController.ZoomBelow100 -= OnZoomBelow100;
    }

    private void HandleRegionClicked(Region region)
    {
        RegionPanel.SetActive(!RegionPanel.activeSelf);
    }
    private void OnZoomBelow100()
    {
        RegionPanel.SetActive(false);
        RegionCanvas.SetActive(false);
    }

    private void OnZoomAbove100()
    {
        RegionCanvas.SetActive(true);
    }
}