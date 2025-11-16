using UnityEngine;

public class CameraToggleController : MonoBehaviour
{
    public MonoBehaviour zoomScript;
    public MonoBehaviour mouseScript;

    private void OnEnable()
    {
        ShopUIController.ShopActivated += HandleCameraDeactivation;
        ShopUIController.ShopDeactivated += HandleCameraActivation;

        UIEvents.OnInventoryActive += HandleCameraDeactivation;
        UIEvents.OnInventoryDeactivated += HandleCameraActivation;

        UIEvents.OnPauseMenuActive += HandleCameraDeactivation;
        UIEvents.OnPauseMenuDeactivated += HandleCameraActivation;
    }
    private void OnDisable()
    {
        ShopUIController.ShopActivated -= HandleCameraDeactivation;
        ShopUIController.ShopDeactivated -= HandleCameraActivation;

        UIEvents.OnInventoryActive -= HandleCameraDeactivation;
        UIEvents.OnInventoryDeactivated -= HandleCameraActivation;

        UIEvents.OnPauseMenuActive -= HandleCameraDeactivation;
        UIEvents.OnPauseMenuDeactivated -= HandleCameraActivation;
    }

    private void HandleCameraDeactivation()
    {
        bool newState = false;

        zoomScript.enabled = newState;
        mouseScript.enabled = newState;
    }

    private void HandleCameraActivation()
    {
        bool newState = true;

        zoomScript.enabled = newState;
        mouseScript.enabled = newState;
    }
}
