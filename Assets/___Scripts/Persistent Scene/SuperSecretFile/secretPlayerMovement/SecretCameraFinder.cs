using Unity.Cinemachine;
using UnityEngine;

public class SecretCameraFinder : MonoBehaviour
{
    void Start()
    {
        var cam = GetComponent<CinemachineCamera>();
        GameObject player = GameObject.FindGameObjectWithTag("SecretPlayer");

        if (player != null && cam != null)
            cam.Follow = player.transform;
        else
            Debug.LogWarning("CameraTargetFinder: Could not find Player or CinemachineCamera component.");
    }
}
