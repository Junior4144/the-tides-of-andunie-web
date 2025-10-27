using UnityEngine;
using Unity.Cinemachine;
public class CameraShakeManager : MonoBehaviour
{
    public static CameraShakeManager instance;
    [SerializeField]
    private AudioClip shakeImpactSound;

    private void Start()
    {
        if (instance == null) instance = this;
    }

    public void CameraShake(CinemachineImpulseSource impulseSource, float shakeForce)
    {
        impulseSource.GenerateImpulseWithForce(shakeForce);
        PlayImpactSound();
    }
    private void PlayImpactSound()
    {
        if (shakeImpactSound != null)
            AudioSource.PlayClipAtPoint(shakeImpactSound, transform.position, 0.1f);
        else
            Debug.LogError("ShakeImpactSound is Null. Playing no Sound");
    }
}
