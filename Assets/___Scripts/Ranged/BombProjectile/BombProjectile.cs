using System.Collections;
using UnityEngine;

public class BombProjectile : MonoBehaviour
{
    [SerializeField] private float velocity = 17;
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private GameObject hitEffectPrefab;
    [SerializeField] private GameObject explosionSoundPrefab;
    [SerializeField] private PirateAttributes pirateAttributes;
    [SerializeField] private float maxTravelDistance = 10f;
    [SerializeField] private float detonationDelay = 0.9f;
    [HideInInspector] public float power;

    private Rigidbody2D rb;
    private Vector2 startPosition;
    private bool hasDealtDamage;
    private bool isDetonating;
    private BombAnimator animator;

    private void Awake()
    {
        animator = GetComponentInChildren<BombAnimator>();
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        startPosition = transform.position;
    }

    private void FixedUpdate()
    {
        if (isDetonating) return;

        if (HasReachedMaxDistance())
            BeginDetonation();
        else
            rb.linearVelocity = transform.up * velocity;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log($"Projectile hit {collision.gameObject.name}");

        if (collision.gameObject.CompareTag("Player"))
        {
            HandlePlayerHit(collision.transform.position);
            return;
        }

        BeginDetonation();
    }

    private bool HasReachedMaxDistance()
    {
        return Vector2.Distance(startPosition, transform.position) >= maxTravelDistance;
    }

    private void HandlePlayerHit(Vector2 hitPosition)
    {
        if (hasDealtDamage) return;

        hasDealtDamage = true;
        SpawnHitEffect(hitPosition);
        DetonateImmediately();
    }

    private void BeginDetonation()
    {
        isDetonating = true;
        rb.linearVelocity = Vector2.zero;
        animator.TriggerAttack();
        StartCoroutine(DetonateAfterDelay());
    }

    private void DetonateImmediately()
    {
        isDetonating = true;
        SpawnExplosionEffects();
        Destroy(gameObject);
    }

    private IEnumerator DetonateAfterDelay()
    {
        yield return new WaitForSeconds(detonationDelay);
        SpawnExplosionEffects();
        Destroy(gameObject);
    }

    private void SpawnExplosionEffects()
    {
        Instantiate(explosionPrefab, transform.position, transform.rotation);
        Instantiate(explosionSoundPrefab, transform.position, Quaternion.identity);
    }

    private void SpawnHitEffect(Vector2 targetPosition)
    {
        Vector2 direction = (targetPosition - (Vector2)rb.transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.Euler(0f, 0f, angle + 90f);
        Instantiate(hitEffectPrefab, targetPosition, rotation);
    }
}
