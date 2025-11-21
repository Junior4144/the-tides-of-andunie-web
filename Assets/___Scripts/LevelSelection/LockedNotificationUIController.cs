using UnityEngine;

public class LockedNotificationUIController : MonoBehaviour
{
    [SerializeField] private GameObject popupPrefab;
    [SerializeField] private Transform worldCanvas;
    [SerializeField] private float cooldownTime = .3f;

    private bool canShowPopup = true;

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
        if (!canShowPopup)
            return;

        // Spawn popup
        var popup = Instantiate(popupPrefab, worldCanvas);
        popup.transform.position = worldPos;

        // Start cooldown to prevent spam
        StartCoroutine(PopupCooldown());
    }

    private System.Collections.IEnumerator PopupCooldown()
    {
        canShowPopup = false;
        yield return new WaitForSeconds(cooldownTime);
        canShowPopup = true;
    }
}
