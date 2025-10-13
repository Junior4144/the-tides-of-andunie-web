using Unity.Cinemachine;
using UnityEngine;

public class CameraTargetFinder : MonoBehaviour
{
    void Start()
    {
        var cam = GetComponent<CinemachineCamera>();
        cam.Follow = PlayerManager.Instance.gameObject.transform;
    }
}
