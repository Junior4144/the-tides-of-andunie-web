using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.AI;
using System;

public class SquadImpulseController : MonoBehaviour
{
    [Header("Impulse Settings")]
    [SerializeField] private float _squadImpulseForce = 10f;
    [SerializeField] private float _squadParentForceMultiplier = 0.1f;
    [SerializeField] private float _impulseDuration = 0.3f;
    [SerializeField] [Range(0f, 1f)] private float _squadDirectionWeight = 0.7f;
    [SerializeField] [Range(0f, 1f)] private float _individualDirectionWeight = 0.3f;
    [SerializeField] private float _centralImpactMultiplier = 1.3f;
    [SerializeField] private float _centralImpactRadius = 3f;
    [SerializeField] private float _minFallOffMultiplier = 0.2f;
    [SerializeField] private float _maxFallOffDistance = 5f;
    

    [Header("Effects")]
    [SerializeField] private ParticleSystem _impulseParticlePrefab;
    [SerializeField] private AudioClip _impulseSound;

    private float _impulseTimer = 0f;
    private NavMeshAgent agent;

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

    public void InitiateSquadImpulse(Vector2 contactPoint, Vector2 impulseDirection)
    {

        ApplyImpulseToUnits(impulseDirection, contactPoint);
        ApplyImpulseToSquad(impulseDirection);
        SpawnParticles(contactPoint, impulseDirection);
        PlaySound(contactPoint);

        _impulseTimer = _impulseDuration;
    }

    private void ApplyImpulseToUnits(Vector2 squadDirection, Vector2 contactPoint)
    {
        float contactDistanceFromCenter = Vector2.Distance(contactPoint, transform.position);

        float centralBonusMultiplier = contactDistanceFromCenter <= _centralImpactRadius ? _centralImpactMultiplier : 1f;

        _squadMemberRigidbodies.Where(rb => rb).ToList().ForEach(rb =>
        {
            Vector2 individualDirection = CalculateDirection(rb.position, contactPoint);

            Vector2 blendedDirection = (
                squadDirection * _squadDirectionWeight +
                individualDirection * _individualDirectionWeight
            ).normalized;

            float falloff = CalcualteFallOffMultiplier(Vector2.Distance(rb.position, contactPoint));
            float finalForce = falloff * _squadImpulseForce * _centralImpactMultiplier;

            rb.linearVelocity = Vector2.zero;
            rb.AddForce(blendedDirection * finalForce, ForceMode2D.Impulse);
        });
    }

    private float CalcualteFallOffMultiplier(float distance) =>
        Mathf.Lerp(1f, _minFallOffMultiplier, Mathf.Clamp01(distance / _maxFallOffDistance));
    
    private void ApplyImpulseToSquad(Vector2 impulseDirection) =>
        transform.position = (Vector2)transform.position + (impulseDirection * _squadImpulseForce * _squadParentForceMultiplier);

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