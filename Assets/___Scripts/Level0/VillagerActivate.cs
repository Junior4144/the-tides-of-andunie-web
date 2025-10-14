using UnityEngine;

public class VillagerActivate : MonoBehaviour
{
    public GameObject group;

    private Camera _camera;


    private void LateUpdate() =>

        _camera = CameraManager.Instance.GetComponent<Camera>();

    private void Update()
    {
        if (!_camera) return;

        if (CheckCameraAllBoundary(_camera.WorldToScreenPoint(group.transform.position), 175f))
            group.SetActive(true);

    }
    public bool CheckCameraAllBoundary(Vector2 screenPosition, float padding)
    {
        return screenPosition.x > -padding &&
               screenPosition.x < Screen.width + padding &&
               screenPosition.y > -padding &&
               screenPosition.y < Screen.height + padding;
    }
}
