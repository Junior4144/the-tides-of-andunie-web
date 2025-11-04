using Unity.Cinemachine;
using UnityEngine;

public class CameraTargetFinder : MonoBehaviour
{
    void Start()
    {
        var cam = GetComponent<CinemachineCamera>();
        if (PlayerManager.Instance?.gameObject.transform) cam.Follow = PlayerManager.Instance.gameObject.transform;

    }
}
