using Unity.Cinemachine;
using UnityEngine;

public class ExplosionCameraShakeController : MonoBehaviour
{
    private CinemachineImpulseSource _impulseSource;
    private bool hasImpulsed = false;

    private void Awake()
    {
        _impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (hasImpulsed) return;
        hasImpulsed = true;
        Debug.Log("Explosion Shake Occured");
        _impulseSource.GenerateImpulseWithForce(2f);
    }
}
