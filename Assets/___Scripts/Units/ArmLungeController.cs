using System.Collections;
using UnityEngine;

public class ArmLungeController : MonoBehaviour
{
    [Header("Control Mode")]
    [Tooltip("Autonomous: Acts independently. Commanded: Controlled by parent (for synchronized spins)")]
    [SerializeField] private bool isCommandedMode = false;

    [Header("Player Detection (Autonomous Mode)")]
    [SerializeField] private float attackRange = 5f;
    [SerializeField] private float attackCooldown = 2f;

    [Header("Sweep Settings")]
    [SerializeField] private SweepMode sweepMode = SweepMode.SingleSweep;

    [Header("Single Sweep Settings (Arc Mode)")]
    [SerializeField] private float sweepArcDegrees = 120f;
    [SerializeField] private float sweepStartAngle = -60f;
    [SerializeField] private float sweepDuration = 0.3f;
    [SerializeField] private float retractDuration = 0.5f;

    [Header("Full Rotation Settings (Spin Mode)")]
    [SerializeField] private float spinRotations = 3f;
    [SerializeField] private float spinDuration = 0.5f;

    [Header("Audio")]
    [SerializeField] private AudioClip attackSound;

    private Quaternion originalLocalRotation;
    private AudioSource audioSource;
    private Coroutine currentRotationCoroutine;
    private float attackCooldownTimer = 0f;

    private enum SweepMode { None, SingleSweep, FullRotation }
    private enum ArmState { Idle, Attacking, CommandedSpin }
    private ArmState state = ArmState.Idle;

    private bool isAttacking = false;

    void Start()
    {
        originalLocalRotation = transform.localRotation;
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            Debug.Log($"[ArmLunge - {gameObject.name}] Added AudioSource component");
        }

        if (audioSource != null)
        {
            audioSource.enabled = true;
            audioSource.playOnAwake = false;
        }
    }

    void Update()
    {
        // Update cooldown timer
        if (attackCooldownTimer > 0)
        {
            attackCooldownTimer -= Time.deltaTime;
        }

        // Only detect player in autonomous mode
        if (!isCommandedMode && !isAttacking && attackCooldownTimer <= 0 && PlayerManager.Instance)
        {
            float d = Vector2.Distance(PlayerManager.Instance.transform.position, transform.position);

            if (d <= attackRange)
            {
                Debug.Log($"[ArmLunge - {gameObject.name}] Player in range, starting attack");
                StartAttack();
            }
        }
    }

    private void StartAttack()
    {
        if (sweepMode == SweepMode.None)
        {
            Debug.LogWarning($"[ArmLunge - {gameObject.name}] SweepMode is None, no attack will occur!");
            return;
        }

        isAttacking = true;
        state = ArmState.Attacking;

        // Play attack sound
        if (audioSource != null && attackSound != null)
        {
            audioSource.PlayOneShot(attackSound);
        }

        // Start rotation
        StartSweepRotation();
    }

    private void StartSweepRotation()
    {
        // Stop any existing rotation
        if (currentRotationCoroutine != null)
        {
            StopCoroutine(currentRotationCoroutine);
        }

        // Start appropriate rotation based on mode
        switch (sweepMode)
        {
            case SweepMode.SingleSweep:
                currentRotationCoroutine = StartCoroutine(RotateSingleSweepRoutine());
                break;

            case SweepMode.FullRotation:
                currentRotationCoroutine = StartCoroutine(RotateFullSpinRoutine(spinDuration, spinRotations));
                break;

            case SweepMode.None:
            default:
                // No rotation
                EndAttack();
                break;
        }
    }

    private IEnumerator RotateSingleSweepRoutine()
    {
        yield return StartCoroutine(RotateSingleSweep());
        EndAttack();
    }

    private IEnumerator RotateFullSpinRoutine(float duration, float rotations)
    {
        yield return StartCoroutine(RotateFullSpin(duration, rotations));
        EndAttack();
    }

    private IEnumerator RotateSingleSweep()
    {
        // Forward sweep
        float elapsed = 0f;

        while (elapsed < sweepDuration)
        {
            float progress = elapsed / sweepDuration;
            float angle = sweepStartAngle + (sweepArcDegrees * progress);

            transform.localRotation = Quaternion.Euler(0, 0, angle);

            elapsed += Time.deltaTime;
            yield return null;
        }

        // Ensure we end at the exact final angle
        float finalAngle = sweepStartAngle + sweepArcDegrees;
        transform.localRotation = Quaternion.Euler(0, 0, finalAngle);

        // Retract sweep (reverse rotation)
        elapsed = 0f;

        while (elapsed < retractDuration)
        {
            float progress = elapsed / retractDuration;
            // Go from finalAngle back to sweepStartAngle
            float angle = finalAngle - (sweepArcDegrees * progress);

            transform.localRotation = Quaternion.Euler(0, 0, angle);

            elapsed += Time.deltaTime;
            yield return null;
        }

        // Ensure we end at the exact start angle
        transform.localRotation = Quaternion.Euler(0, 0, sweepStartAngle);
    }

    private IEnumerator RotateFullSpin(float duration, float rotations)
    {
        float elapsed = 0f;
        float startAngle = transform.localRotation.eulerAngles.z;
        float totalAngle = rotations * 360f;

        while (elapsed < duration)
        {
            float progress = elapsed / duration;
            float angle = startAngle + (totalAngle * progress);

            transform.localRotation = Quaternion.Euler(0, 0, angle);

            elapsed += Time.deltaTime;
            yield return null;
        }
    }

    private void EndAttack()
    {
        state = ArmState.Idle;
        isAttacking = false;
        attackCooldownTimer = attackCooldown;
        transform.localRotation = originalLocalRotation;
    }

    #region Public Methods for Commanded Mode

    /// <summary>
    /// Call this from parent controller to make all arms spin synchronously
    /// </summary>
    public void StartSynchronizedSpin(float duration, float rotations)
    {
        if (!isCommandedMode)
        {
            Debug.LogWarning($"[ArmLunge - {gameObject.name}] StartSynchronizedSpin called but not in commanded mode!");
            return;
        }

        state = ArmState.CommandedSpin;
        isAttacking = true;

        if (currentRotationCoroutine != null)
        {
            StopCoroutine(currentRotationCoroutine);
        }

        currentRotationCoroutine = StartCoroutine(SynchronizedSpinRoutine(duration, rotations));
    }

    private IEnumerator SynchronizedSpinRoutine(float duration, float rotations)
    {
        // Play attack sound
        if (audioSource != null && attackSound != null)
        {
            audioSource.PlayOneShot(attackSound);
        }

        yield return StartCoroutine(RotateFullSpin(duration, rotations));

        // Return to idle after spin
        state = ArmState.Idle;
        isAttacking = false;
        transform.localRotation = originalLocalRotation;
    }

    /// <summary>
    /// Stop any current commanded action
    /// </summary>
    public void StopCommandedAction()
    {
        if (currentRotationCoroutine != null)
        {
            StopCoroutine(currentRotationCoroutine);
            currentRotationCoroutine = null;
        }

        state = ArmState.Idle;
        isAttacking = false;
        transform.localRotation = originalLocalRotation;
    }

    #endregion

    public bool IsAttacking => isAttacking;
    public bool IsInCommandedMode => isCommandedMode;
}
