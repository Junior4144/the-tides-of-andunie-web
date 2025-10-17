using UnityEngine;

public class LevelSelectionCameraFinder : MonoBehaviour
{
    Camera _camera;
    void Start()
    {
        _camera = GameManager.Instance.MainCamera;
        if (_camera == null)
        {
            Debug.LogWarning("No MainCamera found in GameManager");
        }

        var canvas = GetComponentInParent<Canvas>();
        if (canvas != null)
        {
            canvas.worldCamera = _camera;
        }
    }


}
