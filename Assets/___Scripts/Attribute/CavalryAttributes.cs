using UnityEngine;


[CreateAssetMenu(fileName = "Cavalry Attributes", menuName = "Scriptable Objects/Cavalry Attributes")]
public class CavalryAttributes : ScriptableObject
{
    public float RotationSpeed;

    public float Health;

    public float DamageAmount;

    public float ReadyDistance;

    public float MinSpeedForTurning;

    public float MaxSpeedForTurning;

    public float ChargeSpeed;
    public float TurnSpeed;
    public float ChargeAngle;

    public float Acceleration = 5f;

    

    [field: Range(0, 1)]
    public float ChanceOfCollectableDrop;
}
