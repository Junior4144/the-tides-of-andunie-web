using UnityEngine;

public class CannonBallRandom : MonoBehaviour
{
    private Rigidbody2D rb;
    public float force;
    private Camera _camera;

    [Header("Random Target Settings")]
    public float minY = -10f;
    public float maxY = 10f;

    private void Awake()
    {
        _camera = Camera.main;
    }
    void Start()
    {
        ShootRandomly();
    }
    void Update()
    {
        DestroyWhenOffScreen();

    }
    public void ShootRandomly()
    {
        Vector3 randomTarget = GetRandomPosition();

        Vector3 direction = randomTarget - transform.position;
        rb.linearVelocity = direction.normalized * force;
    }
    public Vector3 GetRandomPosition()
    {
        rb = GetComponent<Rigidbody2D>();
        Vector3 randomTarget = new Vector3(
            0f,
            Random.Range(minY, maxY),
            0f
        );
        return randomTarget;
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
        if (collision.CompareTag("CannonBall") == false && collision.CompareTag("Enemy") == false)
        {
            Destroy(gameObject);
        }
    }
}
