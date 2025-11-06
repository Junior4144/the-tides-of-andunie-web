using UnityEngine;

public class ArrowProjectile : MonoBehaviour
{
    [HideInInspector] public float ArrowVelocity;

    [SerializeField] Rigidbody2D rb;

    [SerializeField] private string _layerName;

    [SerializeField] GameObject expo;

    public float power;

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

                GameObject expoObject = Instantiate(expo, transform.position, transform.rotation);

                float scale = GetScaleFromPower(power);
                expoObject.transform.localScale = Vector3.one * scale;

                Destroy(gameObject);
            }
        }

        // Destroy the arrow on impact
        if (collision.CompareTag("Building")) Destroy(gameObject);


        

        Destroy(gameObject, 10f);
    }

    private float GetScaleFromPower(float p)
    {
        if (p < 1f) return 1f;
        if (p < 2f) return 2f;
        return 3f;
    }
}
