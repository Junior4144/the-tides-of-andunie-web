using UnityEngine;

public class ArrowProjectile : MonoBehaviour
{
    [HideInInspector] public float ArrowVelocity;

    [SerializeField] private string _layerName;

    [SerializeField] GameObject expo;

    [HideInInspector] public float power;

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
        SpawnExplosion();
        Destroy(gameObject);
    }

    private void SpawnExplosion()
    {
        GameObject expoObject = Instantiate(expo, transform.position, transform.rotation);
        expoObject.transform.localScale = Vector3.one * Mathf.Max(1f, power);
    }

}
