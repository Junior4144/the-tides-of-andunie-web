using UnityEngine;

public class CameraToggleController : MonoBehaviour
{
    [SerializeField] private Vector2 hotspot = new Vector2(16, 16);

    public MonoBehaviour zoomScript;
    public MonoBehaviour mouseScript;

    public Texture2D defaultCursor;
    public Texture2D normalCursor;

    private void OnEnable()
    {
        ShopUIController.ShopActivated += HandleCameraDeactivation;
        ShopUIController.ShopDeactivated += HandleCameraActivation;

        UIEvents.OnPauseMenuActive += HandleCameraDeactivation;
        UIEvents.OnPauseMenuDeactivated += HandleCameraActivation;

        UIEvents.OnPreScreenConfirm += HandleCameraDeactivation;
        UIEvents.OnPreScreenDeactivated += HandleCameraActivation;

        UIEvents.OnTutorialActive += HandleCameraDeactivation;
        UIEvents.OnTutorialDeactivated += HandleCameraActivation;

        UIEvents.OnLSEnterConfirm += HandleCameraDeactivation;
        UIEvents.OnLSEnterDeactivated += HandleCameraActivation;

        UIEvents.EndGamePopUPActive += HandleCameraDeactivation;
    }

    private void OnDisable()
    {
        ShopUIController.ShopActivated -= HandleCameraDeactivation;
        ShopUIController.ShopDeactivated -= HandleCameraActivation;

        UIEvents.OnPauseMenuActive -= HandleCameraDeactivation;
        UIEvents.OnPauseMenuDeactivated -= HandleCameraActivation;

        UIEvents.OnPreScreenConfirm -= HandleCameraDeactivation;
        UIEvents.OnPreScreenDeactivated -= HandleCameraActivation;

        UIEvents.OnTutorialActive -= HandleCameraDeactivation;
        UIEvents.OnTutorialDeactivated -= HandleCameraActivation;

        UIEvents.OnLSEnterConfirm -= HandleCameraDeactivation;
        UIEvents.OnLSEnterDeactivated -= HandleCameraActivation;

        UIEvents.EndGamePopUPActive -= HandleCameraDeactivation;
    }

    private void HandleCameraDeactivation()
    {
        bool newState = false;

        zoomScript.enabled = newState;
        mouseScript.enabled = newState;

        if (PlayerManager.Instance == null) return;

        PlayerManager.Instance.DisableLSPlayerMovement();

        Cursor.SetCursor(null, hotspot, CursorMode.Auto);
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

        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
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

        Cursor.SetCursor(normalCursor, Vector2.zero, CursorMode.Auto);
    }
}
