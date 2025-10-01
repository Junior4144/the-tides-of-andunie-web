using UnityEngine;

public class CannonBallStraight : MonoBehaviour
{
    private Rigidbody2D rb;
    public float force;
    private Camera _camera;

    [SerializeField]
    private EnemyAttribute _enemyAttribute;
    private void Awake()
    {
        _camera = Camera.main;
    }
    void Start()
    {
        ShootStraight();
    }
    void ShootStraight()
    {
        rb = GetComponent<Rigidbody2D>();
        Vector2 direction = Vector2.left;

        rb.linearVelocity = direction * force;
    }

    void Update()
    {
        //DestroyWhenOffScreen();

    }
    private void DestroyWhenOffScreen()
    {
        Vector2 screenPosition = _camera.WorldToScreenPoint(transform.position);

        if (screenPosition.x < 0 ||
            screenPosition.y < 0 ||
            screenPosition.y > _camera.pixelHeight)
        {
            Destroy(gameObject);

        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var healthController = collision.GetComponentInParent<HealthController>();
        if (healthController != null && collision.CompareTag("Player"))
        {
            healthController.TakeDamage(_enemyAttribute.DamageAmount);
        }
        if (collision.CompareTag("CannonBall") == false && 
            collision.CompareTag("Enemy") == false && 
            collision.CompareTag("Building") == false &&
            collision.CompareTag("Fallen Tree") == false
            )
        {
            Destroy(gameObject);
        }
    }
}
