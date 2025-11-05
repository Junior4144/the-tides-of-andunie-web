using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 10f;
    public float acceleration = 20f;

    [Header("Rotation")]
    public float rotateSpeed = 10f;
    public float facingOffsetDegrees = -90f;

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Vector2 moveVelocity;
    private Vector2 lastMoveDir = Vector2.right;

    // external rotation flags
    private bool externalRotationControl = false;
    private float externalRotationAngle;
    private bool externalSnap = false;

    // Combat rotation system
    public bool inCombat = false;
    public float combatEndDelay = 1f;
    private float combatTimer = 0f;

    public bool IsWalking => moveInput.sqrMagnitude > 0.01f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
    }

    void Update()
    {
        moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (moveInput.sqrMagnitude > 1f)
            moveInput.Normalize();

        // ✅ WASD overrides rotation ONLY if NOT in combat
        if (!inCombat && moveInput.sqrMagnitude > 0.01f)
        {
            ReleaseRotationControl();
            lastMoveDir = moveInput;
        }

        // Combat grace timer
        if (inCombat)
        {
            combatTimer -= Time.deltaTime;
            if (combatTimer <= 0f)
                inCombat = false;
        }

        Vector2 targetVel = moveInput * moveSpeed;
        moveVelocity = Vector2.Lerp(moveVelocity, targetVel, acceleration * Time.deltaTime);
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + moveVelocity * Time.fixedDeltaTime);

        if (externalRotationControl)
        {
            if (externalSnap)
            {
                rb.MoveRotation(externalRotationAngle);
                externalSnap = false;
            }
            else
            {
                float angle = Mathf.LerpAngle(rb.rotation, externalRotationAngle, rotateSpeed * Time.fixedDeltaTime);
                rb.MoveRotation(angle);
            }
            return;
        }

        // Movement-based rotation
        if (lastMoveDir.sqrMagnitude > 0.001f)
        {
            float targetAngle = Mathf.Atan2(lastMoveDir.y, lastMoveDir.x) * Mathf.Rad2Deg + facingOffsetDegrees;
            float angle = Mathf.LerpAngle(rb.rotation, targetAngle, rotateSpeed * Time.fixedDeltaTime);
            rb.MoveRotation(angle);
        }
    }

    // --- Rotation API for other scripts --- //
    public void RequestRotationControl() => externalRotationControl = true;
    public void ReleaseRotationControl() => externalRotationControl = false;

    public void SetExternalRotationAngle(float angle, bool snap = false)
    {
        externalRotationAngle = angle;
        externalSnap = snap;
        externalRotationControl = true;
    }

    // ✅ called by attack script
    public void EnterCombat()
    {
        inCombat = true;
        combatTimer = combatEndDelay;
    }
}
