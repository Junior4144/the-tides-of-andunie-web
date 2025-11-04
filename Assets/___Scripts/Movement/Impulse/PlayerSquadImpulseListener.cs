using UnityEngine;

public class PlayerSquadImpulseListener : MonoBehaviour
{
    [SerializeField] private string _layerName = "Enemy";

    private PlayerSquadImpulseController _controller;
    private Rigidbody2D _rb;

    void Start()
    {
        _rb = GetComponentInParent<Rigidbody2D>();
        Debug.Log($"[PlayerSquadImpulseListener] Found rigidbody {_rb?.name}");

        if (PlayerManager.Instance == null)
        {
            Debug.LogError("[PlayerSquadImpulseListener] PlayerManager instance null");
            return;
        }

        GameObject playerObj = PlayerManager.Instance.gameObject;
        if (playerObj == null)
        {
            Debug.LogError("[PlayerSquadImpulseListener] Player gameobject null");
            return;
        }

        Debug.Log($"[PlayerSquadImpulseListener] Found player {playerObj.name}");
        _controller = playerObj.GetComponent<PlayerSquadImpulseController>();

        if (_controller != null)
        {
            _controller.RegisterMember(_rb);
            Debug.Log($"[PlayerSquadImpulseListener] Registered member {gameObject.name}");
        }
        else
        {
            Debug.LogError("[PlayerSquadImpulseListener] Missing controller component");
        }
    }

    void OnDestroy()
    {
        if (_controller != null)
        {
            _controller.UnregisterMember(_rb);
            Debug.Log($"[PlayerSquadImpulseListener] Unregistered member {gameObject.name}");
        }
    }

    private void OnTriggerEnter2D(Collider2D otherCollider)
    {
        Debug.Log($"[PlayerSquadImpulseListener] Trigger entered {otherCollider.gameObject.name}");

        if (_controller == null)
        {
            Debug.Log("[PlayerSquadImpulseListener] No controller found");
            return;
        }

        if (_controller.IsInImpulse())
        {
            Debug.Log("[PlayerSquadImpulseListener] Already in impulse");
            return;
        }

        if (otherCollider.gameObject.name != "ImpulseCollider")
        {
            Debug.Log($"[PlayerSquadImpulseListener] Wrong collider name {otherCollider.gameObject.name}");
            return;
        }

        if (otherCollider.gameObject.layer != LayerMask.NameToLayer(_layerName))
        {
            Debug.Log($"[PlayerSquadImpulseListener] Wrong layer {LayerMask.LayerToName(otherCollider.gameObject.layer)}");
            return;
        }

        Vector2 closestPoint = otherCollider.ClosestPoint(transform.position);
        Vector2 impulseDirection = (transform.position - otherCollider.transform.position).normalized;

        Debug.Log($"[PlayerSquadImpulseListener] Initiating impulse {impulseDirection}");
        _controller.InitiateSquadImpulse(closestPoint, impulseDirection, false);
    }
}
