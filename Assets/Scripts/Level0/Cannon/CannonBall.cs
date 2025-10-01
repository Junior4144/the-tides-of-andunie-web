using UnityEngine;

public class CannonBall : MonoBehaviour
{
    private GameObject player;
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
        ShootTowardsPlayer();
    }

    void Update()
    {
        //DestroyWhenOffScreen();
    }
    public void ShootTowardsPlayer()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");

        Vector3 direction = player.transform.position - transform.position;
        rb.linearVelocity = new Vector2(direction.x, direction.y).normalized * force;
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
