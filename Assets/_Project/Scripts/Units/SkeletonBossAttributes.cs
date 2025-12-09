using UnityEngine;

[CreateAssetMenu(menuName = "Boss/SkeletonBossAttributes")]
public class SkeletonBossAttributes : ScriptableObject
{
    
    [field: SerializeField]
    public float MovementSpeed { get; private set; }
    
    [field: SerializeField]
    public float RotationSpeed { get; private set; }

    [field: SerializeField]
    public float AggroRange { get; private set; }

    [field: SerializeField]
    public float AttackCooldown { get; private set; }

    [field: SerializeField]
    public float LungeForce { get; private set; }

    [field: SerializeField]
    public float Health { get; private set; }

    [field: SerializeField]
    public float ArmHealth { get; private set; }
}
