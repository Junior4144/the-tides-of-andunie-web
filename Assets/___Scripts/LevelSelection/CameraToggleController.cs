using UnityEngine;

public class CameraToggleController : MonoBehaviour
{
    public MonoBehaviour zoomScript;
    public MonoBehaviour mouseScript;

    private void OnEnable()
    {
        UIEvents.OnRequestShopToggle += HandleCameraToggle;
        UIEvents.OnRequestInventoryToggle += HandleCameraToggle;
        UIEvents.OnRequestPauseToggle += HandleCameraToggle;
    }
    private void OnDisable()
    {
        UIEvents.OnRequestShopToggle -= HandleCameraToggle;
        UIEvents.OnRequestInventoryToggle -= HandleCameraToggle;
        UIEvents.OnRequestPauseToggle -= HandleCameraToggle;
    }

    private void HandleCameraToggle()
    {
        bool newState = !zoomScript.enabled;

        zoomScript.enabled = newState;
        mouseScript.enabled = newState;
    }
}
