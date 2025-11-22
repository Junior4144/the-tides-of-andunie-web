using System.Collections;
using UnityEngine;

/// <summary>
/// Example parent controller that manages synchronized arm attacks for the Skeleton Boss
/// </summary>
public class SkeletonBossArmController : MonoBehaviour
{
    [Header("Arm References")]
    [SerializeField] private ArmLungeController[] arms;

    [Header("Synchronized Spin Attack")]
    [SerializeField] private float spinCooldown = 5f;
    [SerializeField] private float spinDuration = 1f;
    [SerializeField] private float spinRotations = 3f;

    [Header("Trigger Settings")]
    [SerializeField] private bool triggerOnPlayerProximity = true;
    [SerializeField] private float triggerRange = 3f;

    private float spinCooldownTimer = 0f;
    private bool isSpinning = false;

    void Update()
    {
        // Update cooldown
        if (spinCooldownTimer > 0)
        {
            spinCooldownTimer -= Time.deltaTime;
        }

        // Check if we should trigger a spin attack
        if (triggerOnPlayerProximity && !isSpinning && spinCooldownTimer <= 0)
        {
            if (PlayerManager.Instance)
            {
                float distance = Vector2.Distance(transform.position, PlayerManager.Instance.transform.position);
                if (distance <= triggerRange)
                {
                    TriggerSynchronizedSpin();
                }
            }
        }
    }

    /// <summary>
    /// Call this to make both arms spin at the same time
    /// </summary>
    public void TriggerSynchronizedSpin()
    {
        if (isSpinning) return;
        if (arms == null || arms.Length == 0)
        {
            Debug.LogWarning("[SkeletonBossArmController] No arms assigned!");
            return;
        }

        // Make sure all arms are in commanded mode
        foreach (var arm in arms)
        {
            if (arm != null && !arm.IsInCommandedMode)
            {
                Debug.LogWarning($"[SkeletonBossArmController] {arm.name} is not in commanded mode!");
            }
        }

        StartCoroutine(SynchronizedSpinRoutine());
    }

    private IEnumerator SynchronizedSpinRoutine()
    {
        isSpinning = true;

        // Start spin on all arms simultaneously
        foreach (var arm in arms)
        {
            if (arm != null)
            {
                arm.StartSynchronizedSpin(spinDuration, spinRotations);
            }
        }

        // Wait for spin to complete
        yield return new WaitForSeconds(spinDuration);

        isSpinning = false;
        spinCooldownTimer = spinCooldown;
    }

    /// <summary>
    /// Stop all arm attacks immediately
    /// </summary>
    public void StopAllArms()
    {
        if (arms == null) return;

        foreach (var arm in arms)
        {
            if (arm != null)
            {
                arm.StopCommandedAction();
            }
        }

        isSpinning = false;
    }

    // Public properties
    public bool IsSpinning => isSpinning;
    public float SpinCooldownRemaining => spinCooldownTimer;

    void OnDrawGizmosSelected()
    {
        // Draw trigger range
        if (triggerOnPlayerProximity)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, triggerRange);
        }
    }
}
