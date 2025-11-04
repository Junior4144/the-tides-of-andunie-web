using Unity.Cinemachine;
using UnityEngine;

public class CameraTargetFinder : MonoBehaviour
{
    void Start()
    {
        var camera = GetComponent<CinemachineCamera>();
        if (camera == null) return;

        var playerTransform = GetPlayerTransform();
        if (playerTransform == null) return;

        camera.Follow = playerTransform;
    }

    private Transform GetPlayerTransform()
    {
        if (PlayerManager.Instance == null) return null;
        if (PlayerManager.Instance.gameObject == null) return null;

        return PlayerManager.Instance.gameObject.transform;
    }
}
