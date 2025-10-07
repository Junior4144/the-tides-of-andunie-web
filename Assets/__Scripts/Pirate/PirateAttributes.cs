using UnityEngine;


[CreateAssetMenu(fileName = "Pirate Attributes", menuName = "Scriptable Objects/Pirate Attributes")]
public class PriateAttributes : ScriptableObject
{
    [field: SerializeField]
    public float RotationSpeed { get; private set; }

    [field: SerializeField]
    public float Health { get; private set; }

    [field: SerializeField]
    public float PlayerAwarenessDistance { get; private set; }

    [field: SerializeField]
    public float DamageAmount { get; private set; }

    [field: SerializeField]
    [field: Range(0, 1)]
    public float ChanceOfCollectableDrop { get; private set; }


}
