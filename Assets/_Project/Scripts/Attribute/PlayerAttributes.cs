using UnityEngine;


[CreateAssetMenu(fileName = "Player Attributes", menuName = "Scriptable Objects/Player Attributes")]
public class PlayerAttributes : ScriptableObject
{
    [field: SerializeField]
    public float MovementSpeed { get; private set; }

    [field: SerializeField]
    public float RotationSpeed { get; private set; }
}

