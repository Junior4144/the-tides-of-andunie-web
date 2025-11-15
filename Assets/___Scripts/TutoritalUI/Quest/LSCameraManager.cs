using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class LSCameraManager : MonoBehaviour
{
    public static LSCameraManager Instance;

    [SerializeField] private PlayerInput playerInput;
    private CameraLSController CameraLSController;
    private CameraZoomController CameraZoomController;

    private void Awake()
    {
        Instance = this;
        CameraLSController = GetComponent<CameraLSController>();
        CameraZoomController = GetComponent<CameraZoomController>();
    }

    public void EnableCamera()
    {
        StartCoroutine(EnableCameraRoutine());
    }

    private IEnumerator EnableCameraRoutine()
    {
        yield return new WaitForSeconds(3f);
        CameraLSController.enabled = true;
        CameraZoomController.enabled = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void DisableCamera()
    {
        CameraLSController.enabled = false;
        CameraZoomController.enabled = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
