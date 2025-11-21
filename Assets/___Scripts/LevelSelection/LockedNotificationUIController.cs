using UnityEngine;

public class LockedNotificationUIController : MonoBehaviour
{
    [SerializeField] private GameObject popupPrefab;
    [SerializeField] private Transform worldCanvas; // World Space Canvas transform

    private void OnEnable()
    {
        CameraLSController.LockedRegionClicked += HandleLockedRegionClicked;
    }

    private void OnDisable()
    {
        CameraLSController.LockedRegionClicked -= HandleLockedRegionClicked;
    }

    private void HandleLockedRegionClicked(Vector2 worldPos)
    {
        // Spawn popup in the world
        var popup = Instantiate(popupPrefab, worldCanvas);

        // Set world position
        popup.transform.position = worldPos;
    }
}
