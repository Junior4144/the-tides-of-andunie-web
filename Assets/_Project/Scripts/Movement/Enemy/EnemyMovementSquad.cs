using UnityEngine;
using System.Linq;

public class EnemyMovementSquad : MonoBehaviour
{
    private Rigidbody2D _leaderRigidbody;
    private Transform _leaderTransform;
    private PlayerAwarenessController _playerAwarenessController;
    private Vector2 _targetDirection;

    private void Awake()
    {
        InitializeLeaderReferences();
        _playerAwarenessController = GetComponent<PlayerAwarenessController>();
        _targetDirection = _leaderTransform.up;
    }

    private void InitializeLeaderReferences()
    {
        UnitIdentifier leaderIdentifier = GetComponentsInChildren<UnitIdentifier>()
            .FirstOrDefault(u => u.IsLeader);

        if (leaderIdentifier == null)
        {
            Debug.LogError($"No leader unit found in squad '{gameObject.name}'. Defaulting to unit.");
            _leaderRigidbody = GetComponentInChildren<Rigidbody2D>();
            _leaderTransform = _leaderRigidbody.transform;
        }
        else
        {
            _leaderTransform = leaderIdentifier.transform;
            _leaderRigidbody = leaderIdentifier.GetComponent<Rigidbody2D>();
        }
    }

    private void Update()
    {
        if (_leaderRigidbody == null) return;
        
        RotateTowardsTarget();
        SetVelocity();
        SyncSquadParentToLeader();
    }

    private void SyncSquadParentToLeader()
    {
        transform.position = _leaderTransform.position;
        transform.rotation = _leaderTransform.rotation;
        
        _leaderTransform.localPosition = Vector3.zero;
        _leaderTransform.localRotation = Quaternion.identity;
    }
    
    private void RotateTowardsTarget()
    {
        if (_targetDirection == Vector2.zero) return;

        Quaternion targetRotation = Quaternion.LookRotation(_leaderTransform.forward, _targetDirection);
        Quaternion rotation = Quaternion.RotateTowards(_leaderTransform.rotation, targetRotation,Time.deltaTime); // REMOVED PIRATE ATTRIBUTE

        _leaderRigidbody.SetRotation(rotation);
    }

    private void SetVelocity()
    {
        _leaderRigidbody.linearVelocity = _leaderTransform.up; // REMOVED PIRATE ATTRIBUTE
    }
}