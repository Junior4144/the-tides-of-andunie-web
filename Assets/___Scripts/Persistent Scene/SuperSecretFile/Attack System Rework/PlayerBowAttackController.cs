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
    [SerializeField] GameObject arrowSprite2;
    [SerializeField] GameObject arrowSprite3;
    [SerializeField] GameObject ChargeSliderUI;
    [SerializeField] GameObject crossHair;

    [Header("Settings")]
    [SerializeField] float arrowSpeedMultiplier = 10f;
    [SerializeField] float minArrowSpeed = 1f;
    [SerializeField] public float maxCharge = 3f;
    [SerializeField] float chargeRate = 3f;
    [SerializeField] float chargeDecreaseRate = 10f;

    public bool IsNormalAiming { get; private set; }
    public bool IsAbilityAiming { get; private set; }

    AudioSource audioSrc;
    Rigidbody2D rb;
    bool isAttacking;
    bool canFire = true;
    public float charge;


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

        // Normal shot (Left Click)
        else if (Input.GetMouseButton(0))
        {
            IsNormalAiming = true;
            IsAbilityAiming = false;
            StartCharging();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            FireBow();
            IsNormalAiming = false;
        }

        // Ability shot (Right Click)
        else if (Input.GetMouseButton(1))
        {
            IsAbilityAiming = true;
            IsNormalAiming = false;
            StartCharging(); // Or StartAbilityCharge()
        }
        else if (Input.GetMouseButtonUp(1))
        {
            FireAbilityBow();
            IsAbilityAiming = false;
        }
    }

    void StartCharging()
    {
        isAttacking = true;
        if(IsNormalAiming) arrowSprite.SetActive(true);
        if (IsAbilityAiming)
        {
            arrowSprite.SetActive(true);
            arrowSprite2.SetActive(true);
            arrowSprite3.SetActive(true);
        }
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

    void FireAbilityBow()
    {
        canFire = false;
        charge = Mathf.Min(charge, maxCharge);

        float speed = Mathf.Max(minArrowSpeed, charge * arrowSpeedMultiplier);
        float baseAngle = Utility.AngleTowardsMouse(transform.position);
        float spreadAngle = 20f; // degrees between each arrow (adjust to taste)
        int arrowCount = 3;

        // Loop for each arrow
        for (int i = 0; i < arrowCount; i++)
        {
            // Center = 0, Left = -1, Right = +1
            int offsetIndex = i - 1;

            // Calculate angle offset
            float angle = baseAngle + offsetIndex * spreadAngle;
            Quaternion rot = Quaternion.Euler(0f, 0f, angle);

            // Spawn position slightly in front of player
            float spawnOffset = 1f;
            Vector3 offset = rot * Vector3.down * spawnOffset;
            Vector3 spawnPos = transform.position + offset;

            // Instantiate arrow
            var arrow = Instantiate(arrowPrefab, spawnPos, rot)
                .GetComponent<ArrowProjectile>();

            arrow.ArrowVelocity = speed;
            arrow.power = charge;
        }

        ApplyImpulse(Quaternion.Euler(0f, 0f, baseAngle));
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
        arrowSprite2.SetActive(false);
        arrowSprite3.SetActive(false);
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
        PlayerManager.Instance.AllowForceChange = true;

        rb.linearVelocity = Vector2.zero;
        rb.AddForce(dir * strength, ForceMode2D.Impulse);

        while (timer < duration)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        // Stop recoil, re-enable control
        PlayerManager.Instance.AllowForceChange = false;
    }
}
