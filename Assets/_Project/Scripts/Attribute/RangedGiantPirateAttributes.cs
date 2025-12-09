using UnityEngine;

[CreateAssetMenu(fileName = "Ranged Giant Attributes", menuName = "Scriptable Objects/Ranged Giant Pirate Attributes")]
public class RangedGiantAttributes : GiantPirateAttributes
{
    [field: SerializeField]
    [Tooltip("Maximum distance to start ranged attacks")]
    public float ReadyDistance { get; private set; } = 15f;

    [field: SerializeField]
    [Tooltip("Minimum distance for ranged attacks (closer = melee)")]
    public float RangedMeleeThreshold { get; private set; } = 6f;

    [field: SerializeField]
    [Tooltip("Distance for melee trigger activation")]
    public float MeleeAttackRange { get; private set; } = 3f;

    [field: SerializeField]
    [Tooltip("Time from animation start until projectile spawns")]
    public float ProjectileSpawnDelay { get; private set; } = 0.67f;

    [field: SerializeField]
    [Tooltip("Cooldown after animation completes before next shot")]
    public float FireCooldown { get; private set; } = 0.33f;

    [field: SerializeField]
    [Tooltip("Knockback force applied to giant when firing cannon")]
    public float CannonImpulseForce { get; private set; } = 10f;

    [field: SerializeField]
    [Tooltip("Duration of cannon recoil effect")]
    public float CannonImpulseDuration { get; private set; } = 0.5f;

    [field: SerializeField]
    public float MeleeAttackAnimDuration { get; private set; } = 1.067f;

    [field: SerializeField]
    public float RangedAttackAnimDuration { get; private set; } = 0.9f;
}
