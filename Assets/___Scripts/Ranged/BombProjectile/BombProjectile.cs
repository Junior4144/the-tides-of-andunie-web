using UnityEngine;

public class BombProjectile : MonoBehaviour
{
    [SerializeField] private float ArrowVelocity;
    [SerializeField] GameObject expo;
    [SerializeField] GameObject hitEffectPrefab;
    [SerializeField] private GameObject expoSound;
    [SerializeField] private PirateAttributes _pirateAttributes;
    [HideInInspector] public float power;

    private Rigidbody2D _rb;

    private bool hasDamage = false;

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

        //check everything

        Debug.Log($"Projectile hit {collision.name}");
        SpawnExplosion();
        SpawnExplosionSound();

        if (collision.TryGetComponent(out IHealthController health))
        {
            if (hasDamage) return;
            hasDamage = true;
            Debug.Log($"[BombProjectile] Damage dealt {_pirateAttributes.DamageAmount}");
            health.TakeDamage(_pirateAttributes.DamageAmount);
            SpawnHitEffect(collision.transform.position);
        }
        Destroy(gameObject);
    }

    void SpawnHitEffect(Vector2 enemyPos)
    {
        Vector2 playerPos = _rb.transform.position;
        Vector2 dir = (enemyPos - playerPos).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Quaternion rot = Quaternion.Euler(0f, 0f, angle + 90f);
        Instantiate(hitEffectPrefab, enemyPos, rot);
    }

    private void SpawnExplosion()
    {
        GameObject expoObject = Instantiate(expo, transform.position, transform.rotation);
        expoObject.transform.localScale = Vector3.one * Mathf.Max(1f, power);
    }

    private void SpawnExplosionSound()
    {
        Instantiate(expoSound, transform.position, Quaternion.identity);
    }
}
