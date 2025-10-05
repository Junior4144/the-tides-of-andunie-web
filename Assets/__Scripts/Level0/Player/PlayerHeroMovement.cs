using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerHeroMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 200f;
    private Vector2 _movementInput;
    private Rigidbody2D body;
    private Impulse impulseScript;

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        impulseScript = GetComponentInChildren<Impulse>();
    }

    void Update()
    {
        if (impulseScript != null && impulseScript.IsInImpulse()) return;

        float yInput = Input.GetAxis("Vertical");
        float xInput = Input.GetAxis("Horizontal");

        if (Math.Abs(yInput) > 0)
            body.linearVelocity = transform.up * yInput * moveSpeed;

        if (Math.Abs(xInput) > 0)
            body.angularVelocity = -xInput * rotationSpeed;
    }
}