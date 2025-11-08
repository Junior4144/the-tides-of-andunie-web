using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.InputSystem.LowLevel.InputStateHistory;
using static UnityEngine.Rendering.DebugUI.Table;

public class PlayerBowAttackController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Transform playerRoot; // GETCOMOOITENT IN PARENT
    [SerializeField] PlayerAnimator animator;
    [SerializeField] AudioClip attackSound;
    [SerializeField] Slider bowPowerSlider;
    [SerializeField] GameObject arrowPrefab;
    [SerializeField] GameObject arrowSprite;
    [SerializeField] GameObject ChargeSliderUI;
    [SerializeField] GameObject crossHair;

    [Header("Settings")]
    [SerializeField] float arrowSpeedMultiplier = 10f;
    [SerializeField] float minArrowSpeed = 1f;
    [SerializeField] float maxCharge = 3f;
    [SerializeField] float chargeRate = 3f;
    [SerializeField] float chargeDecreaseRate = 10f;

    AudioSource audioSrc;
    Rigidbody2D rb;
    bool isAttacking;
    bool canFire = true;
    float charge;

    private void OnEnable()
    {
        ChargeSliderUI.SetActive(true);
        crossHair.SetActive(true);
    }
    private void OnDisable()
    {
        bowPowerSlider.value = 0f;
        charge = 0f;
        ChargeSliderUI.SetActive(false);
        crossHair.SetActive(false);
    }

    public bool IsAttacking => isAttacking;

    void Awake()
    {
        audioSrc = GetComponent<AudioSource>();
        rb = playerRoot.GetComponent<Rigidbody2D>();
        bowPowerSlider.value = 0f;
        bowPowerSlider.maxValue = maxCharge;
        crossHair.SetActive(false);
    }

    void Update()
    {
        if (!canFire) HandleCooldown();
        else if (Input.GetMouseButton(0)) StartCharging();
        else if (Input.GetMouseButtonUp(0)) FireBow();
    }

    void StartCharging()
    {
        isAttacking = true;
        arrowSprite.SetActive(true);
        charge = Mathf.Min(charge + Time.deltaTime * chargeRate, maxCharge);
        bowPowerSlider.value = charge;
    }

    void FireBow()
    {
        canFire = false;
        charge = Mathf.Min(charge, maxCharge);

        float speed = Mathf.Max(minArrowSpeed, charge * arrowSpeedMultiplier);
        float angle = Utility.AngleTowardsMouse(transform.position);
        Quaternion rot = Quaternion.Euler(0f, 0f, angle);

        float spawnOffset = 1f; // how far back you want the arrow
        Vector3 offset = rot * Vector3.down * spawnOffset;
        Vector3 spawnPos = transform.position + offset;

        var arrow = Instantiate(arrowPrefab, spawnPos, rot)
            .GetComponent<ArrowProjectile>();

        arrow.ArrowVelocity = speed;
        arrow.power = charge;

        ApplyImpulse(rot);

        ResetAfterFire();
    }

    void HandleCooldown()
    {
        charge = Mathf.Max(0f, charge - chargeDecreaseRate * Time.deltaTime);
        if (charge == 0f) canFire = true;
        bowPowerSlider.value = charge;
    }

    void ResetAfterFire()
    {
        isAttacking = false;
        arrowSprite.SetActive(false);
        bowPowerSlider.value = 0f;
    }

    public void ApplyImpulse(Quaternion rot)
    {
        Vector2 recoilDir = -(rot * Vector3.up);
        float recoilStrength = Mathf.Lerp(30f, 100f, charge / maxCharge);
        float recoilDuration = 0.1f + (charge / maxCharge) * 0.1f;

        StartCoroutine(ImpulseRoutine(recoilDir, recoilStrength, recoilDuration));
    }

    private IEnumerator ImpulseRoutine(Vector2 dir, float strength, float duration)
    {
        float timer = 0f;

        // Temporarily disable movement
        PlayerManager.Instance.AllowVelocityChange = true;

        rb.linearVelocity = Vector2.zero;
        rb.AddForce(dir * strength, ForceMode2D.Impulse);

        while (timer < duration)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        // Stop recoil, re-enable control
        PlayerManager.Instance.AllowVelocityChange = false;
    }
}
