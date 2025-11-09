using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.AI;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public abstract class BaseSquadImpulseController : MonoBehaviour
{
    [Header("Impulse Settings")]
    [SerializeField] [Range(0f, 1f)] private float _squadDirectionWeight = 0.7f;
    [SerializeField] [Range(0f, 1f)] private float _individualDirectionWeight = 0.3f;
    [SerializeField] private float _centralImpactMultiplier = 2.5f;
    [SerializeField] private float _minFallOffMultiplier = 0.2f;
    [SerializeField] private float _maxFallOffDistance = 10f;

    [Header("Effects")]
    [SerializeField] private ParticleSystem _impulseParticlePrefab;
    [SerializeField] private AudioClip _impulseSound;

    protected float _impulseTimer = 0f;
    protected Rigidbody2D _heroRigidBody;
    protected NavMeshAgent _agent;
    protected List<Rigidbody2D> _squadMemberRigidbodies = new();
    protected AudioSource _audioSource;

    protected virtual void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _audioSource = GetComponent<AudioSource>();
    }

    protected virtual void Update()
    {
        if (_impulseTimer > 0) _impulseTimer -= Time.deltaTime;
        else if (_agent != null) _agent.enabled = true;
    }

    public bool IsInImpulse() => _impulseTimer > 0;

    public void RegisterMember(Rigidbody2D rb)
    {
        if (rb != null && !_squadMemberRigidbodies.Contains(rb))
        {
            _squadMemberRigidbodies.Add(rb);
            Debug.Log($"[BaseSquadImpulseController] Unit registered {rb.gameObject.name}");
        }
        else if (rb != null)
        {
            Debug.LogWarning($"[BaseSquadImpulseController] Duplicate registration {rb.gameObject.name}");
        }
    }

    public void UnregisterMember(Rigidbody2D rb)
    {
        if (rb != null && _squadMemberRigidbodies.Contains(rb))
        {
            _squadMemberRigidbodies.Remove(rb);
            Debug.Log($"[BaseSquadImpulseController] Unit unregistered {rb.gameObject.name}");
            Debug.Log($"[BaseSquadImpulseController] Total members {_squadMemberRigidbodies.Count}");
        }
    }

    protected abstract float GetDashMultiplier(bool isDashing);

    protected abstract float GetAttackMultiplier(bool isAttacking);

    public void InitiateSquadImpulse(
        float impulseforce,
        float _impulseDuration,
        Vector2 contactPoint,
        Vector2 impulseDirection,
        bool isDashing = false,
        bool isAttacking = false,
        bool playSound = true,
        bool spawnParticles = true
    )
    {
        ApplyImpulseToUnits(impulseforce, impulseDirection, contactPoint, isDashing, isAttacking);
        StartCoroutine(AdjustSquadPosition(_impulseDuration));

        if (spawnParticles) SpawnParticles(contactPoint, impulseDirection);
        if (playSound) PlaySound();

        _impulseTimer = _impulseDuration;
    }

    private void ApplyImpulseToUnits(float impulseforce, Vector2 impulseDirection, Vector2 contactPoint, bool isDashing, bool isAttacking)
    {
        _squadMemberRigidbodies.Where(rb => rb).ToList().ForEach(rb =>
        {
            Vector2 individualDirection = (rb.position - contactPoint).normalized;

            Vector2 blendedDirection = BlendVectors(
                impulseDirection, individualDirection,
                _squadDirectionWeight, _individualDirectionWeight
            ).normalized;

            float dashBonusMultiplier = GetDashMultiplier(isDashing);
            float attackBonusMultiplier = GetAttackMultiplier(isAttacking);

            float finalForce =
                CalcualteFallOffMultiplier(Vector2.Distance(rb.position, contactPoint)) *
                CalculateBehindnessBonusMultiplier(rb.position, contactPoint, impulseDirection) *
                impulseforce *
                dashBonusMultiplier *
                attackBonusMultiplier;

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

    private IEnumerator AdjustSquadPosition(float _impulseDuration)
    {
        yield return new WaitForSeconds(_impulseDuration);

        if (_heroRigidBody)
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

    private void PlaySound()
    {
        if (_impulseSound == null)
        {
            Debug.LogWarning("No impulse audio found. Playing nothing.");
            return;
        }

        _audioSource.PlayOneShot(_impulseSound);
    }
}
