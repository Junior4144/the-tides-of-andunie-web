using System.Collections;
using UnityEngine;

public class BombProjectile : MonoBehaviour
{
    [SerializeField] private float ArrowVelocity;
    [SerializeField] GameObject expo;
    [SerializeField] GameObject hitEffectPrefab;
    [SerializeField] private GameObject expoSound;
    [SerializeField] private PirateAttributes _pirateAttributes;
    [SerializeField] private float travelDistance = 5f;
    [SerializeField] private float _attackAnimDuration = .9f;
    [HideInInspector] public float power;

    private Rigidbody2D _rb;
    private Vector2 startPos;
    private bool hasDamage = false;
    private BombAnimator _animator;
    private void Awake()
    {
        _animator = GetComponentInChildren<BombAnimator>();
    }
    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        startPos = transform.position;
    }

    private void FixedUpdate()
    {
        if (Vector2.Distance(startPos, transform.position) < travelDistance)
        {
            _rb.linearVelocity = transform.up * ArrowVelocity;
        }
        else
        {
            _rb.linearVelocity = Vector2.zero;
            _animator.TriggerAttack();
            StartCoroutine(HandleBombSequence());
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log($"Projectile hit {collision.name}");
        if (collision.TryGetComponent(out IHealthController health))
        {
            if (hasDamage) return;
            hasDamage = true;
            SpawnHitEffect(collision.transform.position);
        }

        SpawnExplosion();
        SpawnExplosionSound();

        Destroy(gameObject);
    }

    private IEnumerator HandleBombSequence()
    {
        yield return new WaitForSeconds(_attackAnimDuration);
        SpawnExplosion();
        SpawnExplosionSound();
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
        Instantiate(expo, transform.position, transform.rotation);
    }

    private void SpawnExplosionSound()
    {
        Instantiate(expoSound, transform.position, Quaternion.identity);
    }
}
