using UnityEngine;

public class CameraToggleController : MonoBehaviour
{
    public MonoBehaviour zoomScript;
    public MonoBehaviour mouseScript;

    private void OnEnable()
    {
        ShopUIController.ShopActivated += HandleCameraDeactivation;
        ShopUIController.ShopDeactivated += HandleCameraActivation;

        //UIEvents.OnInventoryActive += HandleCameraDeactivation;
        //UIEvents.OnInventoryDeactivated += HandleCameraActivation;

        UIEvents.OnPauseMenuActive += HandleCameraDeactivation;
        UIEvents.OnPauseMenuDeactivated += HandleCameraActivation;

        UIEvents.OnPreScreenConfirm += HandleCameraDeactivation;
        UIEvents.OnPreScreenDeactivated += HandleCameraActivation;

        UIEvents.OnTutorialActive += HandleCameraDeactivation;
        UIEvents.OnTutorialDeactivated += HandleCameraActivation;

        UIEvents.OnLSEnterConfirm += HandleCameraDeactivation;
        UIEvents.OnLSEnterDeactivated += HandleCameraActivation;
    }

    private void OnDisable()
    {
        ShopUIController.ShopActivated -= HandleCameraDeactivation;
        ShopUIController.ShopDeactivated -= HandleCameraActivation;

        //UIEvents.OnInventoryActive -= HandleCameraDeactivation;
        //UIEvents.OnInventoryDeactivated -= HandleCameraActivation;

        UIEvents.OnPauseMenuActive -= HandleCameraDeactivation;
        UIEvents.OnPauseMenuDeactivated -= HandleCameraActivation;

        UIEvents.OnPreScreenConfirm -= HandleCameraDeactivation;
        UIEvents.OnPreScreenDeactivated -= HandleCameraActivation;

        UIEvents.OnTutorialActive -= HandleCameraDeactivation;
        UIEvents.OnTutorialDeactivated -= HandleCameraActivation;

        UIEvents.OnLSEnterConfirm -= HandleCameraDeactivation;
        UIEvents.OnLSEnterDeactivated -= HandleCameraActivation;
    }

    private void HandleCameraDeactivation()
    {
        bool newState = false;

        zoomScript.enabled = newState;
        mouseScript.enabled = newState;

        if (PlayerManager.Instance == null) return;

       PlayerManager.Instance.DisableLSPlayerMovement();
    }
    private void HandleCameraDeactivation(bool isExit)
    {
        bool newState = false;

        zoomScript.enabled = newState;
        mouseScript.enabled = newState;

        if (PlayerManager.Instance == null)
        {
            Debug.LogError("Unable to find player manager HandleCameraDeactivation");
            return;
        }

        PlayerManager.Instance.DisableLSPlayerMovement();
    }


    private void HandleCameraActivation()
    {
        bool newState = true;

        zoomScript.enabled = newState;
        mouseScript.enabled = newState;

        if (PlayerManager.Instance == null)
        {
            Debug.LogError("Unable to find player manager HandleCameraActivation");
            return;
        }

        PlayerManager.Instance.EnableLSPlayerMovement();
    }
}
