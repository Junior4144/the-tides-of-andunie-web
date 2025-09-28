using UnityEngine;

public class CannonBall : MonoBehaviour
{
    private GameObject player;
    private Rigidbody2D rb;
    public float force;

    private Camera _camera;
    private void Awake()
    {
        _camera = Camera.main;
    }
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");

        Vector3 direction = player.transform.position - transform.position;
        rb.linearVelocity = new Vector2(direction.x, direction.y).normalized * force;
    }

    void Update()
    {
        DestroyWhenOffScreen();
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
}
