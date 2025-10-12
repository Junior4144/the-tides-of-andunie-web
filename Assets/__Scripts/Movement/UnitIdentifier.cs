using UnityEngine;

public class UnitIdentifier : MonoBehaviour
{
    [SerializeField] private bool _isLeader = false;

    public bool IsLeader => _isLeader;
}