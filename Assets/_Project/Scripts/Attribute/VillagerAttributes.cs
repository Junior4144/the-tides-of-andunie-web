using UnityEngine;


[CreateAssetMenu(fileName = "Villager Attributes", menuName = "Scriptable Objects/Villager Attributes")]
public class VillagerAttributes : ScriptableObject
{
    [field: SerializeField]
    public float Speed { get; private set; }

    [field: SerializeField]
    public float RotationSpeed { get; private set; }

    [field: SerializeField]
    public float Health { get; private set; }

    [field: SerializeField]
    public float TargetAwarenessDistance { get; private set; }

    [field: SerializeField]
    public int KillScore { get; private set; }

    [field: SerializeField]
    public float DamageAmount { get; private set; }

    [field: SerializeField]
    [field: Range(0, 1)]
    public float ChanceOfCollectableDrop { get; private set; }

    [field: SerializeField]
    public float ScreenBorder { get; private set; }

    [field: SerializeField]
    public float ObstacleCheckCircleRadius { get; private set; }

    [field: SerializeField]
    public float ObstacleCheckDistance { get; private set; }

    [field: SerializeField]
    public LayerMask ObstacleLayerMask { get; private set; }


}
