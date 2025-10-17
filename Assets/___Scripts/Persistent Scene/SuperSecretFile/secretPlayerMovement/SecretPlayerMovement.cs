using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class SecretPlayerMovemetn : MonoBehaviour
{
    [SerializeField]
    private float _speed;
    private Animator animator;

    [Header("Dash")]
    [SerializeField] private float dashForce = 30f;
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] private float dashCooldown = 0.7f;

    [SerializeField]
    private GameObject swordSoundPrehab;

    private Rigidbody2D _rigidbody;
    private Vector2 _movementInput;
    private Vector2 _smoothMovementInput;
    private Vector2 _movementInputSmoothVelocity;
    public bool _isAttacking = false;
    private bool _isDashing = false;
    private bool _canDash = true;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && _canDash && !_isDashing)
        {
            StartCoroutine(DashCoroutine());
            return;
        }
        if (!_isDashing)
        {
            SetPlayerVelocity();
        }
    }


    private void SetPlayerVelocity()
    {
        _smoothMovementInput = Vector2.SmoothDamp(
            _smoothMovementInput,
            _movementInput,
            ref _movementInputSmoothVelocity,
            0.1f);
        _rigidbody.linearVelocity = _smoothMovementInput * _speed;
    }

    private void OnMove(InputValue inputValue) =>
        _movementInput = inputValue.Get<Vector2>(); 

    private void OnClick(InputValue inputValue)
    {
        if (inputValue.isPressed && !_isAttacking)
        {
            if (animator)
            {
                _isAttacking = true;
                animator.SetBool("IsAttacking", true);
                Instantiate(swordSoundPrehab, transform.position, Quaternion.identity);
                StartCoroutine(ResetAttackAnimation());
            }
            else
            {
                Debug.LogWarning("Animator is Null. Playing no Animation");
            }

            Debug.Log("Click animation triggered!");
        }
    }
    private IEnumerator ResetAttackAnimation()
    {
        Debug.Log("Attack started");

        yield return new WaitForSeconds(0.6f);

        animator.SetBool("IsAttacking", false);
        _isAttacking = false;

        Debug.Log("Attack finished");
    }

    private IEnumerator DashCoroutine()
    {
        _isDashing = true;
        _canDash = false;

        _rigidbody.linearVelocity = Vector2.zero;

        Vector2 dashDirection = _movementInput.normalized;

        if (dashDirection == Vector2.zero)
        {
            dashDirection = _smoothMovementInput.normalized;
            if (dashDirection == Vector2.zero)
                dashDirection = transform.up;
        }

        _rigidbody.AddForce(dashDirection * dashForce, ForceMode2D.Impulse);

        yield return new WaitForSeconds(dashDuration);
        _isDashing = false;

        yield return new WaitForSeconds(dashCooldown);
        _canDash = true;
    }

    public bool CanDash() => _canDash && !_isDashing;

    public bool IsInDash() => _isDashing;
}
