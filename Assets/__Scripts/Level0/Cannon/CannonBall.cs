using Unity.Cinemachine;
using UnityEngine;

public abstract class CannonBall : MonoBehaviour 
{
    [SerializeField]
    private EnemyAttribute _enemyAttribute;

    protected Rigidbody2D rb;

    private CinemachineImpulseSource _impulseSource;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Shoot();
        _impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    private void Update()
    {
        Vector3 viewportPos = Camera.main.WorldToViewportPoint(transform.position);

        float verticalMargin = 1f; // 0.1 = 10% increase

        if (viewportPos.x < 0 || viewportPos.y > 1 + verticalMargin || viewportPos.y < 0 - verticalMargin)
            Destroy(gameObject);
    }

    protected abstract void Shoot();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == null) return;

        if (IsValidBuildingCollision(collision))
            HandleDestroyObject();

        if (collision.CompareTag("Player"))
        {
            HandlePlayerCollision(collision);
            HandleCameraShake();
        }
        
    }

    private bool IsValidBuildingCollision(Collider2D collision)
    {
        if (!collision.CompareTag("Building")) return false;

        return collision.GetComponent<BuildingDestructable>().hasExploded == false;
    }

    void HandlePlayerCollision(Collider2D collision)
    {
        HandleDamage(collision);
        HandleDestroyObject();
    }

    void HandleDamage(Collider2D collision) => 
        collision.GetComponent<HealthController>()?.TakeDamage(_enemyAttribute.DamageAmount);

    void HandleDestroyObject() => Destroy(gameObject);
    void HandleCameraShake() => CameraShakeManager.instance.CameraShake(_impulseSource);
}