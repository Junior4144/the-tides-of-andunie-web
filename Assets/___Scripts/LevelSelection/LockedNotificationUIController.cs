using UnityEngine;

public class LockedNotificationUIController : MonoBehaviour
{
    [SerializeField] private GameObject popupPrefab;
    [SerializeField] private Transform worldCanvas;
    [SerializeField] private float cooldownTime = .3f;
    [SerializeField] private AudioClip errorSound;

    private bool canShowPopup = true;
    private AudioSource _audioSource;
    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }
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
        
        var popup = Instantiate(popupPrefab, worldCanvas);
        popup.transform.position = worldPos;
        
        StartCoroutine(PopupCooldown());
        _audioSource.PlayOneShot(errorSound);
    }

    private System.Collections.IEnumerator PopupCooldown()
    {
        canShowPopup = false;
        yield return new WaitForSeconds(cooldownTime);
        canShowPopup = true;
    }
}
