using UnityEngine;

public class MusketBallProjectile : MonoBehaviour
{
    [SerializeField] private float ArrowVelocity;

    [SerializeField] GameObject hitEffectPrefab;

    [SerializeField] private PirateAttributes _pirateAttributes;

    private Rigidbody2D _rb;

    private bool hasDamage = false;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();

        if (PlayerManager.Instance != null)
        {
            Vector2 direction = (PlayerManager.Instance.transform.position - transform.position).normalized;

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
            transform.rotation = Quaternion.Euler(0, 0, angle);

            _rb.linearVelocity = direction * ArrowVelocity;
        }
        else
        {
            Debug.LogWarning("[MusketBallProjectile] PlayerManager not found, using default forward direction");
            _rb.linearVelocity = transform.up * ArrowVelocity;
        }

        Destroy(gameObject, 4f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log($"Projectile hit {collision.name}");

        if (collision.CompareTag("DeflectCollider"))
        {
            DeflectProjectile(collision);
            return;
        }

        if (collision.TryGetComponent(out HealthController health))
        {
            if (hasDamage) return;
            Debug.Log($"[MusketBallProjectile] Damage dealt {_pirateAttributes.DamageAmount}");
            health.TakeDamage(_pirateAttributes.DamageAmount, DamageType.Ranged);
            hasDamage = true;
            SpawnHitEffect(collision.transform.position);
            Destroy(gameObject);
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("Environment"))
            Destroy(gameObject);
    }

    void DeflectProjectile(Collider2D collision)
    {
        Vector2 incomingDir = _rb.linearVelocity.normalized;

        Vector2 contactPoint = collision.ClosestPoint(transform.position);
        Vector2 normal = ((Vector2)transform.position - contactPoint).normalized;
        Vector2 reflectDir = Vector2.Reflect(incomingDir, normal);

        _rb.linearVelocity = reflectDir * ArrowVelocity;

        float angle = Mathf.Atan2(reflectDir.y, reflectDir.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);

        Debug.Log($"[MusketBallProjectile] Deflected by player attack!");
    }

    void SpawnHitEffect(Vector2 enemyPos)
    {
        Vector2 playerPos = _rb.transform.position;
        Vector2 dir = (enemyPos - playerPos).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Quaternion rot = Quaternion.Euler(0f, 0f, angle + 90f);
        Instantiate(hitEffectPrefab, enemyPos, rot);
    }
}
