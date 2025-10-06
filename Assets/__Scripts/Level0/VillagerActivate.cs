using UnityEngine;

public class VillagerActivate : MonoBehaviour
{
    public GameObject group;
    public Camera _camera;

    //get screenview 
    private void Start()
    {
        //group.SetActive(true);
    }
    private void LateUpdate()
    {
        _camera = Camera.main;
    }
    private void Update()
    {
        //if within cam screen + margin -< setActive
        if (CheckCameraAllBoundary(_camera.WorldToScreenPoint(group.transform.position), 100f))
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
