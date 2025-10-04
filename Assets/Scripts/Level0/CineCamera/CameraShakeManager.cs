using UnityEngine;
using Unity.Cinemachine;
public class CameraShakeManager : MonoBehaviour
{
    public static CameraShakeManager instance;
    [SerializeField]
    private AudioClip shakeImpactSound;


    [SerializeField] private float globalShakeForce = 1f;

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    public void CameraShakeA(CinemachineImpulseSource impulseSource) 
    {
        impulseSource.GenerateImpulseWithForce(globalShakeForce);
        PlayImpactSound();
    }
    private void PlayImpactSound()
    {
        if (shakeImpactSound != null)
        {
            AudioSource.PlayClipAtPoint(shakeImpactSound, transform.position, 0.1f);
        }
        else
        {
            Debug.LogError("ShakeImpactSound is Null. Playing no Sound");
        }
    }
}
