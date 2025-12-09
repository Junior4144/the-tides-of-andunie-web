using UnityEngine;

public struct ImpulseSettings
{
    public float Force;
    public float Duration;
    public bool PlaySound;
    public bool SpawnParticles;
}

public class ImpulseSource : MonoBehaviour
{
    [Header("Impulse Settings")]
    [SerializeField] private float _impulseforce = 30f;
    [SerializeField] private float impulseDuration = 0.5f;
    [SerializeField] private bool playSound = true;
    [SerializeField] private bool spawnParticles = true;

    public ImpulseSettings GetImpulseSettings()
    {
        return new ImpulseSettings
        {
            Force = _impulseforce,
            Duration = impulseDuration,
            PlaySound = playSound,
            SpawnParticles = spawnParticles
        };
    }
}