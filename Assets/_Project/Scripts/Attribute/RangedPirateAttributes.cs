using UnityEngine;

[CreateAssetMenu(fileName = "Ranged Pirate Attributes", menuName = "Scriptable Objects/Ranged Pirate Attributes")]
public class RangedPirateAttributes : PirateAttributes
{
    [Header("Ranged Attack Settings")]
    [field: SerializeField]
    public float ReadyDistance { get; private set; }
}
