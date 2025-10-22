using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerHeroMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float rotationSpeed = 200f;
    
    [Header("Dash")]
    [SerializeField] private float dashForce = 30f;
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] private float dashCooldown = 0.7f;

    [Header("Footstep Settings")]
    [SerializeField] private float footstepInterval = 0.5f;
    
    private Rigidbody2D body;
    private Impulse impulseScript;
    private AldarionWalkingSoundController footstepController;
    
    private bool _isDashing = false;
    private bool _canDash = true;
    private bool _isWalking = false;
    private float _footstepTimer = 0f;

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        impulseScript = GetComponentInChildren<Impulse>();
        footstepController = GetComponentInChildren<AldarionWalkingSoundController>();
    }

    void Update()
    {
        _isWalking = false;

        if (impulseScript != null && impulseScript.IsInImpulse()) return;

        HandleDashingInput();

        if (_isDashing) return;

        HandleWalkingInput();

        _isWalking = body.linearVelocity.magnitude >= 1;
        
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
            body.linearVelocity = transform.up * yInput * moveSpeed;

        if (Math.Abs(xInput) > 0)
            body.angularVelocity = -xInput * rotationSpeed;
    }

    private IEnumerator DashCoroutine()
    {
        _isDashing = true;
        _canDash = false;

        body.linearVelocity = Vector2.zero;
        body.AddForce(transform.up * dashForce, ForceMode2D.Impulse);

        yield return new WaitForSeconds(dashDuration);
        _isDashing = false;

        yield return new WaitForSeconds(dashCooldown);
        _canDash = true;
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
            if (footstepController != null)
                footstepController.PlayFootstep();
            
            _footstepTimer = footstepInterval;
        }
    }

    public bool CanDash() => _canDash && !_isDashing;

    public bool IsInDash() => _isDashing;
}