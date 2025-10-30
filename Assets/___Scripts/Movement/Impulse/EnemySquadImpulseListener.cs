using UnityEngine;

public class EnemySquadImpulseListener : MonoBehaviour
{
    [SerializeField] private string _layerName = "Player";

    private EnemySquadImpulseController _controller;
    private Rigidbody2D _rb;

    void Start()
    {
        if (transform.parent != null && transform.parent.parent != null)
            _rb = transform.parent.parent.GetComponent<Rigidbody2D>();

        _controller = GetComponentInParent<EnemySquadImpulseController>();

        if (_controller != null && _rb != null)
            _controller.RegisterMember(_rb);
        else
        {
            if (_controller == null)
                Debug.LogWarning($"{gameObject.name}: Could not find EnemySquadImpulseController in parent hierarchy.");
            if (_rb == null)
                Debug.LogWarning($"{gameObject.name}: Could not find Rigidbody2D on parent's parent.");
        }
    }

    void OnDestroy()
    {
        if (_controller != null)
            _controller.UnregisterMember(_rb);
    }

    private void OnTriggerEnter2D(Collider2D otherCollider)
    {
        if (_controller == null) return;
        if (_controller.IsInImpulse()) return;
        if (otherCollider.gameObject.name != "ImpulseCollider") return;
        if (otherCollider.gameObject.layer != LayerMask.NameToLayer(_layerName)) return;

        Debug.Log("[EnemySquadImpulseListener] detected ImpulseCollider");

        Vector2 closestPoint = otherCollider.ClosestPoint(transform.position);
        Vector2 impulseDirection = (transform.position - otherCollider.transform.position).normalized;

        _controller.InitiateSquadImpulse(closestPoint, impulseDirection, PlayerManager.Instance.IsInDash());
    }
}
