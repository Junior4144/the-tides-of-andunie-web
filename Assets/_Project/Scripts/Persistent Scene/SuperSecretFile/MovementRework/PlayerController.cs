using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerAttributes _playerAttributes;
    [SerializeField] private PlayerAttackController attackScript;
    [SerializeField] private PlayerBowAttackController bowAttackScript;
    [SerializeField] private float mouseRotationSpeed = .25f;

    private Rigidbody2D PlayerRigidBody;
    private Vector2 movementInput;
    private float movementSpeed;

    public bool IsWalking { get; private set; }

    void Awake()
    {
        PlayerRigidBody = GetComponent<Rigidbody2D>();
    }
    private void OnEnable()
    {
        PlayerStatsManager.OnSpeedChanged += UpdateSpeed;
    }

    void Start()
    {
        movementSpeed = PlayerStatsManager.Instance.Speed;
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
        if (PlayerManager.Instance.AllowForceChange) return;

        PlayerRigidBody.linearVelocity = movementInput * movementSpeed;
        IsWalking = movementInput.magnitude > 0f;

        if (!bowAttackScript.IsAttacking)
        {
            RotatePlayerEightWay();
        }
        else
        {
            RotateHand();
        }
    }

    private void UpdateSpeed(float speed, float _)
    {
        movementSpeed = speed;
    }

    private void RotatePlayerEightWay()
    {
        if (movementInput == Vector2.zero) return;

        float angle = Mathf.Atan2(movementInput.y, movementInput.x) * Mathf.Rad2Deg;

        float snappedAngle = Mathf.Round(angle / 45f) * 45f;

        float currentAngle = PlayerRigidBody.rotation;
        float targetAngle = snappedAngle - 90f;

        bool isDiagonal = Mathf.Abs(movementInput.x) > 0 && Mathf.Abs(movementInput.y) > 0;
        float adjustedRotationSpeed = isDiagonal ? _playerAttributes.RotationSpeed * 2f : _playerAttributes.RotationSpeed;
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
