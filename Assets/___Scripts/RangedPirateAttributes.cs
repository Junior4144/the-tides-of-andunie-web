using UnityEngine;


[CreateAssetMenu(fileName = "Ranged Pirate Attributes", menuName = "Scriptable Objects/Ranged Pirate Attributes")]
public class RangedPirateAttributes : ScriptableObject
{
    [field: SerializeField]
    public float RotationSpeed { get; private set; }

    [field: SerializeField]
    public float Speed { get; private set; }

    [field: SerializeField]
    public float Health { get; private set; }

    [field: SerializeField]
    public float ReadyDistance { get; private set; }

    [field: SerializeField]
    public float DamageAmount { get; private set; }

    [field: SerializeField]
    [field: Range(0, 1)]
    public float ChanceOfCollectableDrop { get; private set; }


}
