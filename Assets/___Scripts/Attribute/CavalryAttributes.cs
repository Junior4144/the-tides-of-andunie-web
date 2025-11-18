using UnityEngine;


[CreateAssetMenu(fileName = "Cavalry Attributes", menuName = "Scriptable Objects/Cavalry Attributes")]
public class CavalryAttributes : ScriptableObject
{
    [Tooltip("How fast the cavalry can rotate, in degrees per second.")]
    [Range(10f, 360f)]
    public float TurningSpeed = 120f;

    public float Health = 300f;

    public float DamageAmount = 50f;

    public float ReadyDistance;

    public float MinSpeedForTurning = 10f;

    public float MaxSpeedForTurning = 45f;

    public float ChargeSpeed = 50f;
    public float PatrollingSpeed = 10f;
    public float ChargeAngle = 20f;

    public float Acceleration = 10f;
    public float ChargeAcceleration = 25f;
    public float Deceleration = 120f;

    public float MinPathStraightnessToAttack = 0.9f;


    public float AttackCoolDown = 5f;
    

    [field: Range(0, 1)]
    public float ChanceOfCollectableDrop;
}
