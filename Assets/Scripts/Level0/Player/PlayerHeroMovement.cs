using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSteeringMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 200f;
    private Vector2 _movementInput;
    private Rigidbody2D body;

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float yInput = Input.GetAxis("Vertical");
        float xInput = Input.GetAxis("Horizontal");

        if (Math.Abs(yInput) > 0)
        {
            body.linearVelocity = transform.up * yInput * moveSpeed;
        }
        if (Math.Abs(xInput) > 0)
        {
            body.angularVelocity = -xInput * rotationSpeed;
        }
    }
    private void OnMove(InputValue inputValue)
    {
        //whenever the player moves
        _movementInput = inputValue.Get<Vector2>(); //
    }
}