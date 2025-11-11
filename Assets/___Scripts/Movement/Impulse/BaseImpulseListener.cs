using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public abstract class BaseImpulseListener : MonoBehaviour
{
    private ImpulseController _impulseController;

    void Awake()
    {
        _impulseController = GetComponentInParent<ImpulseController>();

        if (!_impulseController)
            _impulseController = GetComponent<ImpulseController>();

        if (!_impulseController)
            Debug.LogError("[BaseImpulseListener] ImpulseListener could not find a SquadImpulseController in its parent.", this);
    }

    protected virtual bool ImpulseIsAllowed() => true;

    private void OnTriggerEnter2D(Collider2D otherCollider)
    {
        if (!ImpulseIsAllowed()) return;

        if (!otherCollider.CompareTag("ImpulseSource")) return;

        if (!otherCollider.TryGetComponent<ImpulseSource>(out var impulseSource))
        {
            Debug.LogWarning($"[BaseImpulseListener] Collider tagged 'ImpulseSource' has no ImpulseSource component: {otherCollider.name}", this);
            return;
        }

        Vector2 closestPoint = otherCollider.ClosestPoint(transform.position);
        Vector2 impulseDirection = (transform.position - otherCollider.transform.position).normalized;

        _impulseController.InitiateSquadImpulse(closestPoint, impulseDirection, impulseSource.GetImpulseSettings());
    }  
}