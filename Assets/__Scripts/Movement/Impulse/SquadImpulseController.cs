using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.AI;

public class SquadImpulseController : MonoBehaviour
{
    [Header("Impulse Settings")]
    [SerializeField] private float _squadImpulseForce = 10f;
    [SerializeField] private float _impulseDuration = 0.3f;
    private float _impulseTimer = 0f;
    private NavMeshAgent agent;

    [Header("Effects")]
    [SerializeField] private ParticleSystem _impulseParticlePrefab;
    [SerializeField] private AudioClip _impulseSound;

    private List<Rigidbody2D> _squadMemberRigidbodies = new List<Rigidbody2D>();

    void Awake()
    {

        agent = GetComponent<NavMeshAgent>();

        _squadMemberRigidbodies.AddRange(
            GetComponentsInChildren<Rigidbody2D>()
                .Where(rb => rb.transform != transform)
        );
    }

    void Update()
    {
        if (_impulseTimer > 0)
            _impulseTimer -= Time.deltaTime;
        else
            agent.enabled = true;
    }

    public bool IsInImpulse() => _impulseTimer > 0;

    public void InitiateSquadImpulse(Collider2D enemyUnitCollider)
    {
        agent.enabled = false;

        Vector2 contactPoint = enemyUnitCollider.ClosestPoint(transform.position);
        Vector2 impulseDirection = (transform.position - enemyUnitCollider.transform.position).normalized;

        ApplyImpulseToSquad(impulseDirection, contactPoint);
        SpawnParticles(contactPoint, impulseDirection);
        PlaySound(contactPoint);

        _impulseTimer = _impulseDuration;
    }

    private void ApplyImpulseToSquad(Vector2 direction, Vector2 contactPoint)
    {
        _squadMemberRigidbodies.Where(rb => rb).ToList().ForEach(rb =>
        {
            float distance = Vector2.Distance(rb.position, contactPoint);
            float falloff = 1f / Mathf.Max(distance, 0.5f);

            rb.linearVelocity = Vector2.zero;
            rb.AddForce(_squadImpulseForce * falloff * CalculateDirection(rb.position, contactPoint), ForceMode2D.Impulse);
        });
    }

    private Vector2 CalculateDirection(Vector3 a, Vector3 b) => (a - b).normalized;

    private void SpawnParticles(Vector2 position, Vector2 direction)
    {
        if (_impulseParticlePrefab == null)
        {
            Debug.LogWarning("No impulse particles found. Spawning none.");
            return;
        }

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.Euler(0, 0, angle + 90f);
        Instantiate(_impulseParticlePrefab, position, rotation);
    }

    private void PlaySound(Vector2 position)
    {
        if (_impulseSound == null)
        {
            Debug.LogWarning("No impulse audio found. Playing nothing.");
            return;
        }

        AudioSource.PlayClipAtPoint(_impulseSound, position, 1.0f);
    }
}