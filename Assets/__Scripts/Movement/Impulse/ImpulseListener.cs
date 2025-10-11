
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class ImpulseListener : MonoBehaviour
{
    private SquadImpulseController _squadManager;

    void Awake()
    {
        _squadManager = GetComponentInParent<SquadImpulseController>();

        if (_squadManager == null)
            Debug.LogError("ImpulseListener could not find a SquadImpulseController in its parent.", this);
    }

    private void OnTriggerEnter2D(Collider2D otherCollider)
    {
        if (_squadManager.IsInImpulse()) return;

        if (otherCollider.gameObject.name != "ImpulseCollider") return;

        Vector2 closestPoint = otherCollider.ClosestPoint(transform.position);
        Vector2 impulseDirection = (transform.position - otherCollider.transform.position).normalized;

        _squadManager.InitiateSquadImpulse(closestPoint, impulseDirection);
    }  
}