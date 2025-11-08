using UnityEngine;

public class EnemySquadImpulseListener : MonoBehaviour
{
    [SerializeField] private string _layerName = "Friendly";
    [SerializeField] private float _impulseForce = 16f;

    private EnemySquadImpulseController _controller;
    private Rigidbody2D _rb;



    void Awake()
    {
        if (transform.parent != null)
            _rb = transform.parent.GetComponent<Rigidbody2D>();
        else
            Debug.LogWarning($"[EnemySquadImpulseListener] Parent hierarchy incomplete! parent={transform.parent?.name}");

        _controller = GetComponentInParent<EnemySquadImpulseController>();

        if (_controller != null && _rb != null)
            _controller.RegisterMember(_rb);
        else
        {
            if (_controller == null)
                Debug.LogWarning($"[EnemySquadImpulseListener]: Could not find EnemySquadImpulseController in parent hierarchy.");
            if (_rb == null)
                Debug.LogWarning($"[EnemySquadImpulseListener]: Could not find Rigidbody2D on parent.");
        }
    }

    void OnDestroy()
    {
        if (_controller != null)
            _controller.UnregisterMember(_rb);
    }
    private void Update()
    {
        if (_controller.IsInImpulse())
        {
            ToggleEnemyIgnore(true);
            //_agent.enabled = false;
        }
        else
            ToggleEnemyIgnore(false);
    }

    void IgnoreEnemies(bool ignore)
    {
        int enemyLayer = LayerMask.NameToLayer("Enemy");

        // Ignore collisions between all enemies globally (by layer)
        Physics2D.IgnoreLayerCollision(enemyLayer, enemyLayer, ignore);

        // Also ignore collisions with any tagged "Enemy" (in case they�re on other layers)
        Collider2D myCol = GetComponent<Collider2D>();
        if (!myCol) return;

        foreach (var enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            if (enemy == gameObject) continue;
            var otherCol = enemy.GetComponent<Collider2D>();
            if (otherCol)
                Physics2D.IgnoreCollision(myCol, otherCol, ignore);
        }
    }

    void ToggleEnemyIgnore(bool shouldIgnore)
    {
        IgnoreEnemies(shouldIgnore);
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

        _controller.InitiateSquadImpulse(_impulseForce, closestPoint, impulseDirection, false);
    }
}
