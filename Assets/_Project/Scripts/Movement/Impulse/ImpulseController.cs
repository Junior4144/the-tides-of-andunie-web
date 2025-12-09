using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(AudioSource))]
public class ImpulseController : MonoBehaviour
{
    [Header("Impulse Settings")]
    [SerializeField] [Range(0f, 1f)] private float _squadDirectionWeight = 0.7f;
    [SerializeField] [Range(0f, 1f)] private float _individualDirectionWeight = 0.3f;
    [SerializeField] private float _centralImpactMultiplier = 2.5f;
    [SerializeField] private float _minFallOffMultiplier = 0.2f;
    [SerializeField] private float _maxFallOffDistance = 10f;
    [SerializeField] [Range(0f, 1f)] private float _impulseResistance = 0f;

    [Header("Effects")]
    [SerializeField] private ParticleSystem _impulseParticlePrefab;
    [SerializeField] private AudioClip _impulseSound;

    private float _impulseTimer = 0f;
    private Rigidbody2D _rb;
    private NavMeshAgent _agent;
    private AudioSource _audioSource;

    protected virtual void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _audioSource = GetComponent<AudioSource>();
        _rb = GetComponent<Rigidbody2D>();
    }

    protected virtual void Update()
    {
        if (_impulseTimer > 0) 
            _impulseTimer -= Time.deltaTime;
        else if (_agent != null) 
            _agent.enabled = true;
    }

    public bool IsInImpulse() => _impulseTimer > 0;

    public void InitiateSquadImpulse(
        Vector2 contactPoint,
        Vector2 impulseDirection,
        ImpulseSettings settings
    )
    {
        ApplyImpulse(settings.Force, impulseDirection, contactPoint);

        if (settings.SpawnParticles) SpawnParticles(contactPoint, impulseDirection);
        if (settings.PlaySound) PlaySound();

        _impulseTimer = settings.Duration * (1f - _impulseResistance);
    }

    private void ApplyImpulse(float impulseforce, Vector2 impulseDirection, Vector2 contactPoint)
    {
        if (_agent != null)
            _agent.enabled = false;

        Vector2 individualDirection = (_rb.position - contactPoint).normalized;

        Vector2 blendedDirection = BlendVectors(
            impulseDirection, individualDirection,
            _squadDirectionWeight, _individualDirectionWeight
        ).normalized;

        float finalForce =
            CalcualteFallOffMultiplier(Vector2.Distance(_rb.position, contactPoint)) *
            CalculateBehindnessBonusMultiplier(_rb.position, contactPoint, impulseDirection) *
            impulseforce *
            (1f - _impulseResistance);

        _rb.linearVelocity = Vector2.zero;
        _rb.AddForce(blendedDirection * finalForce, ForceMode2D.Impulse);
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

    private void SpawnParticles(Vector2 position, Vector2 direction)
    {
        if (_impulseParticlePrefab == null)
        {
            Debug.LogWarning("[BaseImpulseController] No impulse particles found. Spawning none.");
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
            Debug.LogWarning("[BaseImpulseController] No impulse audio found. Playing nothing.");
            return;
        }

        if (_audioSource == null || !_audioSource.enabled) return;

        _audioSource.PlayOneShot(_impulseSound);
    }
}
