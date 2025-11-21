using Unity.Cinemachine;
using UnityEngine;

public abstract class CannonBall : MonoBehaviour 
{

    public GameObject explosion;


    [SerializeField]
    private CannonBallAttributes _cannonBallAttributes;
    [SerializeField]
    private AudioClip _explosionSound;

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
        if (!Camera.main) return;
        Vector3 viewportPos = Camera.main.WorldToViewportPoint(transform.position);

        float verticalMargin = 1f;

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

        if (collision == null)
            return false;
        return collision.GetComponent<BuildingDestructable>().hasExploded == false;
    }

    void HandlePlayerCollision(Collider2D collision)
    {
        HandleDamage(collision);
        HandleDestroyObject();
    }

    void HandleDamage(Collider2D collision)
    {
        if (collision.TryGetComponent(out HealthController health))
        {
            health.TakeDamage(_cannonBallAttributes.DamageAmount);
        }

    } 
        

    void HandleDestroyObject() => Destroy(gameObject);
    void HandleCameraShake() => CameraShakeManager.instance.CameraShake(_impulseSource, 1f);
}