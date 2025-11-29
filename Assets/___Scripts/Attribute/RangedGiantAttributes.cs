using UnityEngine;

[CreateAssetMenu(fileName = "Ranged Giant Pirate Attributes", menuName = "Scriptable Objects/Ranged Giant Pirate Attributes")]
public class RangedGiantPirateAttributes : GiantPirateAttributes
{
    [Header("Ranged Attack Settings")]
    [field: SerializeField]
    public float ReadyDistance { get; private set; }
}
