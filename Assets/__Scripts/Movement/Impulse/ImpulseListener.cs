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
        if (otherCollider.gameObject.name == "ImpulseCollider")
            _squadManager.InitiateSquadImpulse(otherCollider);
    }
        
}