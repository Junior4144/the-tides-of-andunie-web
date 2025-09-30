using UnityEngine;

public class Impulse : MonoBehaviour
{
    [SerializeField] private float _impulseForce = 5f;
    [SerializeField] private float _impulseDuration = 0.3f;
    [SerializeField] private string _layerName;
    
    private Rigidbody2D _playerRigidbody;
    private float _impulseTimer = 0f;

    void Start()
    {
        _playerRigidbody = GetComponentInParent<Rigidbody2D>();
    }

    void Update()
    {
        if (_impulseTimer > 0f)
        {
            _impulseTimer -= Time.deltaTime;
        }
    }

    public void OnTriggerEnter2D(Collider2D otherCollider)
    {
        // Only do impulse if the other collider has an impulse script and its on the right layer
        if (otherCollider.GetComponent<Impulse>() && otherCollider.gameObject.layer == LayerMask.NameToLayer(_layerName))
        {
            DoImpulse(otherCollider);
        }
    }

    public void DoImpulse(Collider2D enemyCollider)
    {
        Debug.Log($"Impulse Collision Detected. Source: {transform.parent.gameObject.name}, OtherCollider: {enemyCollider.name}");
        _playerRigidbody.AddForce(ImpulseDirection(enemyCollider) * _impulseForce, ForceMode2D.Impulse);
        _impulseTimer = _impulseDuration;
    }

    public bool IsInImpulse() => _impulseTimer > 0f;

    private Vector2 ImpulseDirection(Collider2D enemyCollider) =>
        (transform.position - enemyCollider.transform.position).normalized;
}