using Unity.Cinemachine;
using UnityEngine;

public class CameraTargetFinder : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        var cam = GetComponent<CinemachineCamera>();

        cam.Follow = GameObject.FindWithTag("Player").transform;
    }
}
