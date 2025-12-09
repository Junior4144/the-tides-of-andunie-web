using UnityEngine;

[CreateAssetMenu(fileName = "Bomber Pirate Attributes", menuName = "Scriptable Objects/Bomber Pirate Attributes")]
public class BomberPirateAttributes : PirateAttributes
{
    [Header("Bomber Attack Settings")]
    [field: SerializeField]
    public float ReadyDistance { get; private set; } = 7f;

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
