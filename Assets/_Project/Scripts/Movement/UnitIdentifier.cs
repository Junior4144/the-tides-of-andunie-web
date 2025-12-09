using UnityEngine;

public class UnitIdentifier : MonoBehaviour
{
    [SerializeField] private bool _isLeader = false;
    [SerializeField] private UnitType _unitType;

    public bool IsLeader => _isLeader;
    public UnitType UnitType => _unitType;
}