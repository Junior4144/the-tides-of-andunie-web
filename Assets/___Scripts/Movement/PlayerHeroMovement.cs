using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(AudioSource))]
public class PlayerHeroMovement : MonoBehaviour
{
    public event Action<Vector2, float, float> OnPlayerDash;

    [Header("Movement")]
    public float moveSpeed = 5f;
    public float rotationSpeed = 200f;

    [Header("Dash")]
    [SerializeField] private float dashForce = 30f;
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] private float dashCooldown = 0.7f;
    [SerializeField] private AudioClip dashSound;

    [Header("Footstep Settings")]
    [SerializeField] private float footstepInterval = 0.5f;

    private Rigidbody2D _rb;
    private PlayerSquadImpulseController _impulseController;
    private AldarionWalkingSoundController _footstepController;
    private AudioSource _audioSource;

    private bool _isDashing = false;
    private bool _canDash = true;
    private bool _isWalking = false;
    private float _footstepTimer = 0f;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _impulseController = GetComponent <PlayerSquadImpulseController>();
        _footstepController = GetComponentInChildren<AldarionWalkingSoundController>();
        _audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        _isWalking = false;

        if (!_impulseController || _impulseController.IsInImpulse()) return;

        HandleDashingInput();

        if (_isDashing) return;

        HandleWalkingInput();

        _isWalking = _rb.linearVelocity.magnitude >= 1;
        
        HandleWalkingSounds();
    }

    private void HandleDashingInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && CanDash())
            StartCoroutine(DashCoroutine());
    }

    private void HandleWalkingInput()
    {
        float yInput = Input.GetAxis("Vertical");
        float xInput = Input.GetAxis("Horizontal");

        if (Math.Abs(yInput) > 0)
            _rb.linearVelocity = moveSpeed * yInput * transform.up;

        if (Math.Abs(xInput) > 0)
            _rb.angularVelocity = -xInput * rotationSpeed;
    }

    private IEnumerator DashCoroutine()
    {
        PlayDashSound();

        _isDashing = true;
        _canDash = false;

        _rb.linearVelocity = Vector2.zero;
        _rb.AddForce(transform.up * dashForce, ForceMode2D.Impulse);
        
        OnPlayerDash?.Invoke(transform.up, dashForce, dashDuration);

        yield return new WaitForSeconds(dashDuration);
        _isDashing = false;

        yield return new WaitForSeconds(dashCooldown);
        _canDash = true;
    }

    private void PlayDashSound()
    {
        if (dashSound != null)
            _audioSource.PlayOneShot(dashSound, volumeScale: 0.0f);
        else
            Debug.LogWarning("[PlayerHeroMovement] Dash sound is null. Playing no sound");
    }

    private void HandleWalkingSounds()
    {
        if (!_isWalking)
        {
            _footstepTimer = 0f;
            return;
        }

        _footstepTimer -= Time.deltaTime;

        if (_footstepTimer <= 0f)
        {
            if (_footstepController != null)
                _footstepController.PlayFootstep();
            
            _footstepTimer = footstepInterval;
        }
    }

    public bool CanDash() => _canDash && !_isDashing;

    public bool IsInDash() => _isDashing;

    public bool IsWalking => _isWalking;
}