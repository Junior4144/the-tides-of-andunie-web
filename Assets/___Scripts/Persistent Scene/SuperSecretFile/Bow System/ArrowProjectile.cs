using UnityEngine;

public class ArrowProjectile : MonoBehaviour
{
    [HideInInspector] public float ArrowVelocity;

    [SerializeField] Rigidbody2D rb;

    [SerializeField] private string _layerName;

    private void Start()
    {
        Destroy(gameObject, 4f);
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = transform.up * ArrowVelocity;
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Optional: only damage certain layers
        if (collision.gameObject.layer == LayerMask.NameToLayer(_layerName))
        {
            if (collision.TryGetComponent(out IHealthController health))
            {
                health.TakeDamage(PlayerStatsManager.Instance.MeleeDamage);
            }
        }

        // Destroy the arrow on impact
        if (collision.CompareTag("Building")) Destroy(gameObject);

        Destroy(gameObject, 10f);
    }
}
