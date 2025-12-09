using System.Collections;
using UnityEngine;

/// <summary>
/// Manages synchronized arm attacks and turn-based single sweeps for the Skeleton Boss
/// </summary>
public class SkeletonBossArmController : MonoBehaviour
{
    [Header("Arm References")]
    [SerializeField] private ArmLungeController[] arms;

    [Header("Synchronized Spin Attack")]
    [SerializeField] private float spinCooldown = 5f;
    [SerializeField] private float spinDuration = 1f;
    [SerializeField] private float spinRotations = 3f;
    [SerializeField] [Range(0f, 1f)] private float spinChance = 0.3f; // 30% chance to do spin instead of single sweep

    [Header("Trigger Settings")]
    [SerializeField] private bool triggerOnPlayerProximity = true;
    [SerializeField] private float triggerRange = 3f;

    [Header("Turn-Based Attack")]
    [SerializeField] private int currentTurn = 0; // Which arm's turn it is (0 or 1)

    private float spinCooldownTimer = 0f;
    private bool isSpinning = false;
    private float lastProximityCheckTime = 0f;
    private float proximityCheckInterval = 0.5f; // Check every 0.5 seconds

    void Update()
    {
        // Update cooldown
        if (spinCooldownTimer > 0)
        {
            spinCooldownTimer -= Time.deltaTime;
        }

        // Check if we should trigger a synchronized spin (randomly)
        if (triggerOnPlayerProximity && !isSpinning && spinCooldownTimer <= 0)
        {
            if (Time.time - lastProximityCheckTime >= proximityCheckInterval)
            {
                lastProximityCheckTime = Time.time;

                if (PlayerManager.Instance)
                {
                    float distance = Vector2.Distance(transform.position, PlayerManager.Instance.transform.position);
                    if (distance <= triggerRange)
                    {
                        // Random chance to do synchronized spin
                        if (Random.value <= spinChance)
                        {
                            TriggerSynchronizedSpin();
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// Check if the specified arm can attack (is it their turn?)
    /// </summary>
    public bool CanArmAttack(int armIndex)
    {
        return armIndex == currentTurn;
    }

    /// <summary>
    /// Called by arm when it completes an attack - switches to other arm's turn
    /// </summary>
    public void OnArmAttackComplete(int armIndex)
    {
        // Switch to the other arm's turn
        currentTurn = (currentTurn + 1) % arms.Length;
        Debug.Log($"[SkeletonBossArmController] Turn switched. Now arm {currentTurn}'s turn");
    }

    /// <summary>
    /// Trigger a synchronized spin on all arms
    /// </summary>
    public void TriggerSynchronizedSpin()
    {
        if (isSpinning) return;
        if (arms == null || arms.Length == 0)
        {
            Debug.LogWarning("[SkeletonBossArmController] No arms assigned!");
            return;
        }

        Debug.Log("[SkeletonBossArmController] Triggering synchronized spin!");
        StartCoroutine(SynchronizedSpinRoutine());
    }

    private IEnumerator SynchronizedSpinRoutine()
    {
        isSpinning = true;

        // Temporarily switch arms to commanded mode
        foreach (var arm in arms)
        {
            if (arm != null)
            {
                arm.SetCommandedMode(true);
            }
        }

        // Small delay to ensure mode is set
        yield return null;

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

        // Switch arms back to autonomous mode
        foreach (var arm in arms)
        {
            if (arm != null)
            {
                arm.SetCommandedMode(false);
            }
        }

        isSpinning = false;
        spinCooldownTimer = spinCooldown;

        Debug.Log("[SkeletonBossArmController] Synchronized spin complete, arms back to autonomous mode");
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

    /// <summary>
    /// Manually trigger a spin (for debugging or special attacks)
    /// </summary>
    public void ForceSpinAttack()
    {
        if (!isSpinning && spinCooldownTimer <= 0)
        {
            TriggerSynchronizedSpin();
        }
    }

    // Public properties
    public bool IsSpinning => isSpinning;
    public float SpinCooldownRemaining => spinCooldownTimer;
    public int CurrentTurn => currentTurn;

    void OnDrawGizmosSelected()
    {
        // Draw trigger range
        if (triggerOnPlayerProximity)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, triggerRange);
        }

        // Draw which arm's turn it is
        if (arms != null && Application.isPlaying)
        {
            for (int i = 0; i < arms.Length; i++)
            {
                if (arms[i] != null)
                {
                    Gizmos.color = (i == currentTurn) ? Color.green : Color.gray;
                    Gizmos.DrawLine(transform.position, arms[i].transform.position);
                }
            }
        }
    }
}
