using UnityEngine;


[CreateAssetMenu(fileName = "Cavalry Attributes", menuName = "Scriptable Objects/Cavalry Attributes")]
public class CavalryAttributes : ScriptableObject
{
    [Tooltip("How fast the cavalry can rotate, in degrees per second.")]
    [Range(10f, 360f)]
    public float TurningSpeed = 120f;

    public float Health = 170f;

    public float DamageAmount = 10f;

    public float MinSpeedForTurning = 10f;

    public float MaxSpeedForTurning = 45f;

    public float ChargeSpeed = 50f;
    public float PatrollingSpeed = 10f;
    public float ChargeAngle = 20f;

    public float Acceleration = 10f;
    public float ChargeAcceleration = 25f;
    public float Deceleration = 120f;

    [Range(0.1f, 1.0f)]
    public float MinPathStraightnessToAttack = 0.75f;

    public float AttackCoolDown = 5f;

    [Tooltip("The distance threshold at which the cavalry will consider itself reached the patrol point")]
    public float PatrolPointReachedThreshold = 5.0f;

    [Tooltip("Penalty multiplier for targets behind the cavalry. Higher values = stronger preference for forward targets.")]
    [Range(0.1f, 1.0f)]
    public float TargetBehindnessPenalty = 0.5f;

    [Tooltip("Time in seconds to detect if the cavalry is stuck.")]
    [Range(0.1f, 10.0f)]
    public float StuckDetectionTime = 1.0f;
}
