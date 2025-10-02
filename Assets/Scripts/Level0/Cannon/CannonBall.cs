using UnityEngine;

public abstract class CannonBall : MonoBehaviour 
{
    [SerializeField]
    private EnemyAttribute _enemyAttribute;

    protected Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Shoot();
    }
    protected abstract void Shoot();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            HandleDamage(collision);
            Destroy(gameObject);
        }
    }
    void HandleDamage(Collider2D collision) => 
        collision.GetComponentInParent<HealthController>()?.TakeDamage(_enemyAttribute.DamageAmount);
}