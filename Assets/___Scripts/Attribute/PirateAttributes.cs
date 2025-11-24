using UnityEngine;


[CreateAssetMenu(fileName = "Pirate Attributes", menuName = "Scriptable Objects/Pirate Attributes")]
public class PirateAttributes : ScriptableObject
{
    [field: SerializeField]
    public float RotationSpeed { get; private set; }
    
    [field: SerializeField]
    public float MovementSpeed { get; private set; }

    [field: SerializeField]
    public float Acceleration { get; private set; }

    [field: SerializeField]
    public float Health { get; private set; }

    [field: SerializeField]
    public float DamageAmount { get; private set; }

    [field: SerializeField]
    public float ReadyDistance { get; private set; }

    [field: SerializeField]
    [field: Range(0, 1)]
    public float ChanceOfCollectableDrop { get; private set; }

    [Header("Bomber Runaway Settings")]
    [field: SerializeField]
    public float RunBackDistance { get; private set; } = 5f;

    [field: SerializeField]
    public float RunBackSpeedMultiplier { get; private set; } = 2f;

    [field: SerializeField]
    [field: Range(0, 1)]
    public float SafetyDistanceMultiplier { get; private set; } = 0.8f;

    [field: SerializeField]
    public int RaycastCount { get; private set; } = 5;

    [field: SerializeField]
    public float RaycastSpread { get; private set; } = 90f;
}
