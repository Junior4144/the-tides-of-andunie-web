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
    
    private Rigidbody2D body;
    private Impulse impulseScript;
    
    private bool _isDashing = false;
    private bool _canDash = true;

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        impulseScript = GetComponentInChildren<Impulse>();
    }

    void Update()
    {
        if (impulseScript != null && impulseScript.IsInImpulse()) return;

        if (Input.GetKeyDown(KeyCode.LeftShift) && _canDash && !_isDashing)
        {
            StartCoroutine(DashCoroutine());
            return;
        }

        if (!_isDashing)
        {
            float yInput = Input.GetAxis("Vertical");
            float xInput = Input.GetAxis("Horizontal");

            if (Math.Abs(yInput) > 0)
                body.linearVelocity = transform.up * yInput * moveSpeed;

            if (Math.Abs(xInput) > 0)
                body.angularVelocity = -xInput * rotationSpeed;
        }
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

    public bool CanDash() => _canDash && !_isDashing;

    public bool IsInDash() => _isDashing;
}