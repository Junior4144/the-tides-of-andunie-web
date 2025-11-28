using UnityEngine;

[CreateAssetMenu(fileName = "Ranged Giant Attributes", menuName = "Scriptable Objects/Ranged Giant Pirate Attributes")]
public class RangedGiantAttributes : GiantPirateAttributes
{
    [Header("Ranged Attack Settings")]
    [field: SerializeField]
    public float ReadyDistance { get; private set; }

    // [Header("Giant Attack Settings")]
    // [field: SerializeField]
    // public float DamageDelay { get; private set; } = 0.62f;

    // [field: SerializeField]
    // public float DamageRange { get; private set; } = 4f;

    // [field: SerializeField]
    // public float ImpulseForce { get; private set; } = 250f;

    // [field: SerializeField]
    // public float ImpulseDuration { get; private set; } = 0.5f;
}
