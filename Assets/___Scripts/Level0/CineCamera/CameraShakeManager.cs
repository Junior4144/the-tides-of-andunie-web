using UnityEngine;
using Unity.Cinemachine;
public class CameraShakeManager : MonoBehaviour
{
    public static CameraShakeManager instance;
    [SerializeField]
    private AudioClip shakeImpactSound;
    private AudioSource audioSource;

    private void Start()
    {
        if (instance == null) instance = this;
        audioSource = GetComponent<AudioSource>();
    }

    public void CameraShake(CinemachineImpulseSource impulseSource, float shakeForce)
    {
        impulseSource.GenerateImpulseWithForce(shakeForce);
        PlayImpactSound();
    }
    private void PlayImpactSound()
    {
        if (shakeImpactSound != null)
            audioSource.PlayOneShot(shakeImpactSound, 0.1f);
        else
            Debug.LogError("ShakeImpactSound is Null. Playing no Sound");
    }
}
