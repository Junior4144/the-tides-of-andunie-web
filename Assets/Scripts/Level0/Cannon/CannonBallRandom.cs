using UnityEngine;
using UnityEngine.Events;

public class CannonBallRandom : MonoBehaviour
{
    private Rigidbody2D rb;
    public float force;
    private Camera _camera;

    [Header("Random Target Settings")]
    public float minY = -10f;
    public float maxY = 10f;

    [SerializeField]
    private EnemyAttribute _enemyAttribute;
    private void Awake()
    {
        _camera = Camera.main;
    }
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        ShootRandomly();
    }
    void Update()
    {
        //DestroyWhenOffScreen();

    }
    public void ShootRandomly()
    {
        Vector3 randomTarget = GetRandomPosition();

        rb.linearVelocity = (randomTarget - transform.position).normalized * force;
    }
    public Vector3 GetRandomPosition()
    {
        return new Vector3(
            0f,
            Random.Range(minY, maxY),
            0f
            );
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

        if (collision.CompareTag("Player"))
        {
            Destroy(gameObject);
        }

    }
}
