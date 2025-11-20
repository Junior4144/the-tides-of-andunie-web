using UnityEngine;

public class ArrowProjectile : MonoBehaviour
{
    [HideInInspector] public float ArrowVelocity;

    [SerializeField] private string _layerName;
    [SerializeField] GameObject expo;
    [SerializeField] private GameObject expoSound;
    [SerializeField] GameObject hitEffectPrefab;

    [HideInInspector] public float power;
    [HideInInspector] public float maxPower;

    private Rigidbody2D _rb;
    private bool AlreadyActivated;

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
        if (AlreadyActivated) return;
        AlreadyActivated = true;
        SpawnExplosion();
        SpawnExplosionSound();
        if(collision.CompareTag("Enemy")) SpawnHitEffect(collision.transform.position);
        Destroy(gameObject);
    }

    private void SpawnExplosion()
    {
        GameObject expoObject = Instantiate(expo, transform.position, transform.rotation);
        expoObject.transform.localScale = Vector3.one * Mathf.Max(1f, power);

        ExplosionDamageController explosion = expoObject.GetComponentInChildren<ExplosionDamageController>();
        if (explosion != null)
        {
            explosion.Power = power;
            explosion.MaxPower = maxPower;
        }
    }

    private void SpawnExplosionSound()
    {
        Instantiate(expoSound, transform.position, Quaternion.identity);
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
