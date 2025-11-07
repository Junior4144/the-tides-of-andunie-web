using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private PlayerAttackController attackScript;
    [SerializeField] private PlayerBowAttackController bowAttackScript;
    [SerializeField] private float rotationSpeed = 8f;
    //[SerializeField] private float rotationSnapBuffer = 0.12f;
    private float lastTurnTime = 0f;


    private Rigidbody2D PlayerRigidBody;
    private Vector2 movementInput;

    void Awake()
    {
        PlayerRigidBody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {

        // Gather input
        float moveX = 0f;
        float moveY = 0f;

        if (Input.GetKey(KeyCode.W)) moveY += 1f;
        if (Input.GetKey(KeyCode.S)) moveY -= 1f;
        if (Input.GetKey(KeyCode.A)) moveX -= 1f;
        if (Input.GetKey(KeyCode.D)) moveX += 1f;

        movementInput = new Vector2(moveX, moveY);

        // Normalize diagonal movement
        if (movementInput.magnitude > 1f)
            movementInput.Normalize();
    }

    void FixedUpdate()
    {
        if (PlayerManager.Instance.IsInImpulse()) return;
        
        PlayerRigidBody.linearVelocity = movementInput * speed;

        if (!attackScript.IsAttacking && !bowAttackScript.IsAttacking)
        {
            RotatePlayerEightWay();
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
        float adjustedRotationSpeed = isDiagonal ? rotationSpeed * 1.5f : rotationSpeed;
        float newAngle = Mathf.LerpAngle(currentAngle, targetAngle, adjustedRotationSpeed * Time.fixedDeltaTime);
        PlayerRigidBody.MoveRotation(newAngle);
    }
}

//private void RotatePlayerEightWay()
//{
//    if (movementInput == Vector2.zero)
//        return;

//    float targetAngle = transform.eulerAngles.z;

//    bool isDiagonal =
//        (movementInput.y > 0 && movementInput.x != 0) ||
//        (movementInput.y < 0 && movementInput.x != 0);

//    float currentRotationSpeed = isDiagonal ? 20f : 10f;

//    // 8-direction rotation angles
//    if (movementInput.y > 0 && movementInput.x > 0) targetAngle = -45f;
//    else if (movementInput.y > 0 && movementInput.x < 0) targetAngle = 45f;
//    else if (movementInput.y < 0 && movementInput.x > 0) targetAngle = -135f;
//    else if (movementInput.y < 0 && movementInput.x < 0) targetAngle = 135f;
//    else if (movementInput.y > 0) targetAngle = 0f;
//    else if (movementInput.y < 0) targetAngle = 180f;
//    else if (movementInput.x > 0) targetAngle = -90f;
//    else if (movementInput.x < 0) targetAngle = 90f;

//    // Smooth rotation
//    Quaternion targetRot = Quaternion.Euler(0, 0, targetAngle);
//    transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, rotationSpeed * Time.fixedDeltaTime);
//}