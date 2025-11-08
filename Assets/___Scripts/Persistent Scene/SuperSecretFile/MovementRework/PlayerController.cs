using System.Collections;
using UnityEngine;
using UnityEngine.LowLevel;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private PlayerAttackController attackScript;
    [SerializeField] private PlayerBowAttackController bowAttackScript;
    [SerializeField] private float rotationSpeed = 8f;
    [SerializeField] private float mouseRotationSpeed = .25f;

    private Rigidbody2D PlayerRigidBody;
    private Vector2 movementInput;

    void Awake()
    {
        PlayerRigidBody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float moveX = 0f;
        float moveY = 0f;

        if (Input.GetKey(KeyCode.W)) moveY += 1f;
        if (Input.GetKey(KeyCode.S)) moveY -= 1f;
        if (Input.GetKey(KeyCode.A)) moveX -= 1f;
        if (Input.GetKey(KeyCode.D)) moveX += 1f;

        movementInput = new Vector2(moveX, moveY);

        if (movementInput.magnitude > 1f)
            movementInput.Normalize();
    }

    void FixedUpdate()
    {
        if (PlayerManager.Instance.IsInImpulse()) return;
        if (PlayerManager.Instance.AllowVelocityChange) return;
        
        PlayerRigidBody.linearVelocity = movementInput * speed;

        //Debug.Log($"Attack: {attackScript.IsAttacking}, Bow: {bowAttackScript.IsAttacking}");
        //!attackScript.IsAttacking &&
        if ( !bowAttackScript.IsAttacking)
        {
            RotatePlayerEightWay();
        }
        else
        {
            RotateHand();
        }

        
    }

    private void RotatePlayerEightWay()
    {
        if (movementInput == Vector2.zero) return;

        // Calculate angle based on input
        float angle = Mathf.Atan2(movementInput.y, movementInput.x) * Mathf.Rad2Deg;

        // Snap to nearest 45° for 8-direction rotation
        float snappedAngle = Mathf.Round(angle / 45f) * 45f;

        // Smoothly interpolate between current and target rotation
        float currentAngle = PlayerRigidBody.rotation;
        float targetAngle = snappedAngle - 90f;

        bool isDiagonal = Mathf.Abs(movementInput.x) > 0 && Mathf.Abs(movementInput.y) > 0;
        float adjustedRotationSpeed = isDiagonal ? rotationSpeed * 2f : rotationSpeed;
        float newAngle = Mathf.LerpAngle(currentAngle, targetAngle, adjustedRotationSpeed * Time.fixedDeltaTime);
        PlayerRigidBody.MoveRotation(newAngle);
    }
    void RotateHand()
    {
        float targetAngle = Utility.AngleTowardsMouse(transform.position);
        float smoothAngle = Mathf.LerpAngle(PlayerRigidBody.rotation, targetAngle, mouseRotationSpeed);
        PlayerRigidBody.MoveRotation(smoothAngle);
    }

}
