
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.AI;
using System;
using System.Collections;

public class SquadImpulseController : MonoBehaviour
{
    [Header("Impulse Settings")]
    [SerializeField] private float _squadImpulseForce = 10f;
    [SerializeField] private float _impulseDuration = 0.3f;
    [SerializeField] [Range(0f, 1f)] private float _squadDirectionWeight = 0.7f;
    [SerializeField] [Range(0f, 1f)] private float _individualDirectionWeight = 0.3f;
    [SerializeField] private float _centralImpactMultiplier = 4f;
    [SerializeField] private float _minFallOffMultiplier = 0.2f;
    [SerializeField] private float _maxFallOffDistance = 5f;
    [SerializeField] private float _dashMultiplier = 3f;
    

    [Header("Effects")]
    [SerializeField] private ParticleSystem _impulseParticlePrefab;
    [SerializeField] private AudioClip _impulseSound;

    private float _impulseTimer = 0f;
    private Rigidbody2D _heroRigidBody;
    private NavMeshAgent agent;

    private List<Rigidbody2D> _squadMemberRigidbodies = new List<Rigidbody2D>();

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();

        _squadMemberRigidbodies.AddRange(
            GetComponentsInChildren<Rigidbody2D>()
                .Where(rb => rb.transform != transform)
        );

        _heroRigidBody = GetComponentsInChildren<UnitIdentifier>()
            .First(unit => unit.IsLeader)
            .GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (_impulseTimer > 0) _impulseTimer -= Time.deltaTime;
        else agent.enabled = true;
    }

    public bool IsInImpulse() => _impulseTimer > 0;

    public void InitiateSquadImpulse(Vector2 contactPoint, Vector2 impulseDirection, bool isDashing)
    {
        ApplyImpulseToUnits(impulseDirection, contactPoint, isDashing);
        StartCoroutine(AdjustSquadPosition());
        SpawnParticles(contactPoint, impulseDirection);
        PlaySound(contactPoint);

        _impulseTimer = _impulseDuration;
    }

    private void ApplyImpulseToUnits(Vector2 impulseDirection, Vector2 contactPoint, bool isDashing)
    {
        float contactDistanceFromCenter = Vector2.Distance(contactPoint, transform.position);

        _squadMemberRigidbodies.Where(rb => rb).ToList().ForEach(rb =>
        {
            Vector2 individualDirection = (rb.position - contactPoint).normalized;

            Vector2 blendedDirection = BlendVectors(
                impulseDirection, individualDirection,
                _squadDirectionWeight, _individualDirectionWeight
            ).normalized;

            float dashBonusMultiplier = isDashing ? _dashMultiplier : 1f;

            float finalForce =
                CalcualteFallOffMultiplier(Vector2.Distance(rb.position, contactPoint)) *
                CalculateBehindnessBonusMultiplier(rb.position, contactPoint, impulseDirection) *
                _squadImpulseForce *
                dashBonusMultiplier;

            rb.linearVelocity = Vector2.zero;
            rb.AddForce(blendedDirection * finalForce, ForceMode2D.Impulse);
        });
    }

    private Vector2 BlendVectors(Vector2 v1, Vector2 v2, float a1, float a2) => a1 * v1 + a2 * v2;

    private float CalculateBehindnessBonusMultiplier(Vector2 unitPosition, Vector2 contactPoint, Vector2 impulseDirection)
    {
        Vector2 contactToUnit = (contactPoint - unitPosition).normalized;

        // transforms Dot() range [-1, 1] -> [0, 1] 
        float behindnessFactor = (Vector2.Dot(contactToUnit, impulseDirection) + 1f) / 2f;

        return Mathf.Lerp(1f, _centralImpactMultiplier, behindnessFactor);
    }

    private float CalcualteFallOffMultiplier(float distance) =>
        Mathf.Lerp(1f, _minFallOffMultiplier, Mathf.Clamp01(distance / _maxFallOffDistance));

    private IEnumerator AdjustSquadPosition()
    {
        yield return new WaitForSeconds(_impulseDuration);
        
        transform.position = _heroRigidBody.transform.position;
    }

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
