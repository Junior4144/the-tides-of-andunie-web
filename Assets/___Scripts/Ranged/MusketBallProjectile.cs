using UnityEngine;

public class MusketBallProjectile : MonoBehaviour
{
    [SerializeField] private float ArrowVelocity;

    [SerializeField] GameObject hitEffectPrefab;

    [SerializeField] private RangedPirateAttributes _pirateAttributes;

    private Rigidbody2D _rb;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();   
        Destroy(gameObject, 4f);
    }

    private void FixedUpdate()
    {
        _rb.linearVelocity = transform.up * ArrowVelocity;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (!collision.CompareTag("Player")) return;

        if (collision.TryGetComponent(out IHealthController health))
        {
            Debug.Log($"[MusketBallProjectile] Damage dealt {_pirateAttributes.DamageAmount}");
            health.TakeDamage(_pirateAttributes.DamageAmount);
            SpawnHitEffect(collision.transform.position);
            Destroy(gameObject);
        }
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
