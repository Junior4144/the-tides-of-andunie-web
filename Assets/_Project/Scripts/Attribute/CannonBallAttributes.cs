using UnityEngine;


[CreateAssetMenu(fileName = "Cannon Ball Attributes", menuName = "Scriptable Objects/Cannon Ball Attributes")]
public class CannonBallAttributes : ScriptableObject
{
    [field: SerializeField]
    public float DamageAmount { get; private set; }
}
