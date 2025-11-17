using System;
using UnityEngine;

public class RegionZoomController : MonoBehaviour
{
    [SerializeField]
    public float threshold = 200f;

    private Camera cam;

    public static event Action ZoomBelowThreshold;
    public static event Action ZoomAboveThreshold;

    private LSPlayerMovement playerMovement;

    private void Start()
    {
        cam = CameraManager.Instance.GetCamera();
        playerMovement = PlayerManager.Instance.gameObject.GetComponent<LSPlayerMovement>();
    }

    private void Update()
    {
        if (cam.orthographicSize <= threshold)
        {
            if (playerMovement == null) return;
            playerMovement.enabled = true;
            ZoomBelowThreshold?.Invoke();
        }

        if (cam.orthographicSize > threshold)
        {
            if (playerMovement == null) return;
            playerMovement.enabled = false;
            ZoomAboveThreshold?.Invoke();
        }
    }
}