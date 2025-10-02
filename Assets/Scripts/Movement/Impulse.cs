using UnityEngine;

public class Impulse : MonoBehaviour
{
    [SerializeField] private float _impulseForce = 5f;
    [SerializeField] private float _impulseDuration = 0.3f;
    [SerializeField] private string _layerName;
    public ParticleSystem _impulseParticlePrefab;


    private Rigidbody2D _playerRigidbody;
    private float _impulseTimer = 0f;

    void Start()
    {
        _playerRigidbody = GetComponentInParent<Rigidbody2D>();
    }

    void Update()
    {
        if (_impulseTimer > 0f)
            _impulseTimer -= Time.deltaTime;
    }

    public void OnTriggerEnter2D(Collider2D otherCollider)
    {
        if (otherCollider.gameObject.name == "ImpulseCollider" && otherCollider.gameObject.layer == LayerMask.NameToLayer(_layerName))
        {
            DoImpulse(otherCollider);
        }
    }

    public void DoImpulse(Collider2D enemyCollider)
    {
        _playerRigidbody.AddForce(ImpulseDirection(enemyCollider) * _impulseForce, ForceMode2D.Impulse);
        _impulseTimer = _impulseDuration;
        SpawnImpulseParticles(enemyCollider);
    }

    public bool IsInImpulse() => _impulseTimer > 0f;

    private Vector2 ImpulseDirection(Collider2D enemyCollider) =>
        (transform.position - enemyCollider.transform.position).normalized;

    private void SpawnImpulseParticles(Collider2D enemyCollider) =>
        Instantiate(
            _impulseParticlePrefab,
            enemyCollider.ClosestPoint(transform.position),
            CalculateRotation(ImpulseDirection(enemyCollider))
        );

    private Quaternion CalculateRotation(Vector2 impulseDir) => Quaternion.Euler(0f, 0f, VectorToAngle(impulseDir) - 90f);

    private float VectorToAngle(Vector2 vector) => Mathf.Atan2(-vector.y, -vector.x) * Mathf.Rad2Deg;
}