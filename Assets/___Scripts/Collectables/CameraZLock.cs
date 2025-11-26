using Unity.Cinemachine;
using UnityEngine;

public class CameraZLock : MonoBehaviour
{
    public float lockedZ = -10;
    CinemachineCamera vcam;

    void Awake()
    {
        vcam = GetComponent<CinemachineCamera>();
    }

    void LateUpdate()
    {
        if (vcam != null)
        {
            var pos = vcam.transform.position;
            if (pos.z != lockedZ)
            {
                pos.z = lockedZ;
                vcam.transform.position = pos;
            }
        }
    }
}