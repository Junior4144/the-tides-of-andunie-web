using System;
using UnityEngine;

public class RegionZoomController : MonoBehaviour
{
    private Camera cam;

    public static event Action ZoomBelow100;
    public static event Action ZoomAbove100;

    private LSPlayerMovement playerMovement;

    private void Start()
    {
        cam = CameraManager.Instance.GetCamera();
        playerMovement = PlayerManager.Instance.gameObject.GetComponent<LSPlayerMovement>();
    }

    private void Update()
    {
        if (cam.orthographicSize <= 100f)
        {
            if (playerMovement == null) return;
            playerMovement.enabled = true;
            ZoomBelow100?.Invoke();
        }

        if (cam.orthographicSize > 100f)
        {
            if (playerMovement == null) return;
            playerMovement.enabled = false;
            ZoomAbove100?.Invoke();
        }
    }
}