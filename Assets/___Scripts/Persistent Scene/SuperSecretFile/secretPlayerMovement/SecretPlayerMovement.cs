using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class SecretPlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float _speed = 5f;

    [Header("Dash")]
    [SerializeField] private float dashForce = 30f;
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] private float dashCooldown = 0.7f;

    [Header("Attack")]
    [SerializeField] private GameObject swordSoundPrefab;

    private Rigidbody2D _rigidbody;
    private Animator animator;

    private Vector2 _movementInput;
    private Vector2 _smoothMovementInput;
    private Vector2 _movementInputSmoothVelocity;

    private bool _isAttacking = false;
    private bool _isDashing = false;
    private bool _canDash = true;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        // --- DASH ---
        if (Input.GetKeyDown(KeyCode.LeftShift) && _canDash && !_isDashing)
        {
            StartCoroutine(DashCoroutine());
            return;
        }

        // --- MOVE ---
        if (!_isDashing)
            SetPlayerVelocity();
    }

    private void SetPlayerVelocity()
    {
        _smoothMovementInput = Vector2.SmoothDamp(
            _smoothMovementInput,
            _movementInput,
            ref _movementInputSmoothVelocity,
            0.1f);

        // mouse-relative basis
        Vector2 forward = GetMouseDir();
        Vector2 right = new Vector2(forward.y, -forward.x);

        // remap WASD around mouse
        Vector2 desired =
            right * _smoothMovementInput.x +
            forward * _smoothMovementInput.y;

        _rigidbody.linearVelocity = desired * _speed;
    }

    private Vector2 GetMouseDir()
    {
        Vector2 mouseScreen = Mouse.current.position.ReadValue();
        Vector3 world = Camera.main.ScreenToWorldPoint(mouseScreen);
        Vector2 dir = (world - transform.position);

        return dir.sqrMagnitude > 0.0001f ? dir.normalized : Vector2.up;
    }

    private void OnMove(InputValue inputValue) =>
        _movementInput = inputValue.Get<Vector2>();

    private void OnClick(InputValue inputValue)
    {
        if (!inputValue.isPressed || _isAttacking) return;
        if (animator)
        {
            _isAttacking = true;
            animator.SetBool("IsAttacking", true);
            if (swordSoundPrefab)
                Instantiate(swordSoundPrefab, transform.position, Quaternion.identity);
            StartCoroutine(ResetAttackAnimation());
        }
    }

    private IEnumerator ResetAttackAnimation()
    {
        yield return new WaitForSeconds(0.6f);
        animator.SetBool("IsAttacking", false);
        _isAttacking = false;
    }

    private IEnumerator DashCoroutine()
    {
        _isDashing = true;
        _canDash = false;
        _rigidbody.linearVelocity = Vector2.zero;

        Vector2 fwd = GetMouseDir();
        Vector2 right = new Vector2(fwd.y, -fwd.x);
        Vector2 inputDir = (right * _movementInput.x + fwd * _movementInput.y).normalized;
        Vector2 dashDir = inputDir == Vector2.zero ? fwd : inputDir;

        _rigidbody.AddForce(dashDir * dashForce, ForceMode2D.Impulse);

        yield return new WaitForSeconds(dashDuration);
        _isDashing = false;

        yield return new WaitForSeconds(dashCooldown);
        _canDash = true;
    }

    public bool CanDash() => _canDash && !_isDashing;
    public bool IsInDash() => _isDashing;
}
