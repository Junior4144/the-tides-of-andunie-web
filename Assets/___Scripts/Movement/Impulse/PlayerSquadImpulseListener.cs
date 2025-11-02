using UnityEngine;

public class PlayerSquadImpulseListener : MonoBehaviour
{
    [SerializeField] private string _layerName = "Enemy";

    private PlayerSquadImpulseController _controller;
    private Rigidbody2D _rb;

    void Start()
    {
        _rb = GetComponentInParent<Rigidbody2D>();

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            _controller = playerObj.GetComponent<PlayerSquadImpulseController>();

            if (_controller != null)
                _controller.RegisterMember(_rb);
            else
                Debug.LogWarning($"{gameObject.name}: Player found but PlayerSquadImpulseController component is missing.");
        }
        else
            Debug.LogWarning($"{gameObject.name}: Could not find Player via tag.");
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

        Debug.Log("[PlayerSquadImpulseListener] detected ImpulseCollider");

        Vector2 closestPoint = otherCollider.ClosestPoint(transform.position);
        Vector2 impulseDirection = (transform.position - otherCollider.transform.position).normalized;

        _controller.InitiateSquadImpulse(closestPoint, impulseDirection, false);
    }
}
