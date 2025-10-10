using Unity.Cinemachine;
using UnityEngine;

public class CameraTargetFinder : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        var cam = GetComponent<CinemachineCamera>();

        cam.Follow = PlayerManager.Instance.gameObject.transform;
    }
}
