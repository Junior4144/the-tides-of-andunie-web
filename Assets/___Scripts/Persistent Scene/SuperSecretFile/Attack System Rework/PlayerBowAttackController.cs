using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBowAttackController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Transform playerRoot;
    [SerializeField] PlayerAnimator animator;
    [SerializeField] AudioClip attackSound;
    [SerializeField] Slider bowPowerSlider;
    [SerializeField] GameObject arrowPrefab;
    [SerializeField] GameObject[] arrowSprites;
    [SerializeField] GameObject ChargeSliderUI;
    [SerializeField] GameObject crossHair;

    [Header("Settings")]
    [SerializeField] float arrowSpeedMultiplier = 10f;
    [SerializeField] float minArrowSpeed = 1f;
    [SerializeField] public float maxCharge = 3f;
    [SerializeField] float chargeRate = 3f;
    [SerializeField] float chargeDecreaseRate = 10f;
    [SerializeField] float minChargeRequired = 1;
    [SerializeField] float arrowSpawnOffset = 1f;
    public bool IsNormalAiming { get; private set; }
    public bool IsAbilityAiming { get; private set; }
    public bool IsAttacking { get; private set; }

    AudioSource audioSrc;
    Rigidbody2D rb;
    bool canFire = true;

    [HideInInspector]
    public float charge;

    void Awake()
    {
        audioSrc = GetComponent<AudioSource>();
        rb = playerRoot.GetComponent<Rigidbody2D>();
        InitUI();
    }

    private void OnEnable()
    {
        SetUIActive(true);
    }

    private void OnDisable()
    {
        ResetCharge();
        CancelShot();
        SetUIActive(false);
    }

    // ---------------- UPDATE ----------------
    void Update()
    {
        if (!canFire) { HandleCooldown(); return; }

        if (Input.GetMouseButton(0)) HandleAiming(normal: true);
        else if (Input.GetMouseButtonUp(0)) Fire(isAbility: false);

        if (Input.GetMouseButton(1)) HandleAiming(normal: false);
        else if (Input.GetMouseButtonUp(1)) Fire(isAbility: true);
    }

    // ---------------- CHARGING ----------------
    void HandleAiming(bool normal)
    {
        IsNormalAiming = normal;
        IsAbilityAiming = !normal;
        IsAttacking = true;

        ToggleArrowSprites(normal ? 1 : 3, true);
        charge = Mathf.Min(charge + Time.deltaTime * chargeRate, maxCharge);
        bowPowerSlider.value = charge;
    }

    // ---------------- FIRING ----------------
    void Fire(bool isAbility)
    {
        Vector2 aimDir = Utility.DirectionTowardsMouse(transform.position);
        Vector2 facingDir = playerRoot.up;

        if (Vector2.Dot(facingDir, aimDir) < 0.2f)
        {
            CancelShot();
            return;
        }

        if (charge < minChargeRequired)
        {
            CancelShot();
            return;
        }

        canFire = false;
        charge = Mathf.Min(charge, maxCharge);

        if (isAbility) FireSpreadShot();
        else FireSingleShot();

        ResetAfterFire();
    }

    void FireSingleShot()
    {
        SpawnArrow(Utility.AngleTowardsMouse(transform.position), charge);
        ApplyImpulse(Utility.RotationTowardsMouse(transform.position));
    }

    void FireSpreadShot()
    {
        float baseAngle = Utility.AngleTowardsMouse(transform.position);
        const float spreadAngle = 20f;
        const int arrowCount = 3;

        for (int i = 0; i < arrowCount; i++)
        {
            float angle = baseAngle + (i - 1) * spreadAngle;
            SpawnArrow(angle, charge);
        }

        ApplyImpulse(Quaternion.Euler(0f, 0f, baseAngle));
    }

    // ---------------- HELPERS ----------------
    void SpawnArrow(float angle, float power)
    {
        float speed = Mathf.Max(minArrowSpeed, power * arrowSpeedMultiplier);
        Quaternion rot = Quaternion.Euler(0f, 0f, angle);
        Vector3 spawnPos = transform.position + rot * Vector3.down * arrowSpawnOffset;

        var arrow = Instantiate(arrowPrefab, spawnPos, rot).GetComponent<ArrowProjectile>();
        arrow.ArrowVelocity = speed;
        arrow.power = power;
    }

    void HandleCooldown()
    {
        charge = Mathf.Max(0f, charge - chargeDecreaseRate * Time.deltaTime);
        bowPowerSlider.value = charge;
        if (charge == 0f) canFire = true;
    }

    void ApplyImpulse(Quaternion rot)
    {
        Vector2 recoilDir = -(rot * Vector3.up);
        float recoilStrength = Mathf.Lerp(30f, 100f, charge / maxCharge);
        float recoilDuration = 0.1f + (charge / maxCharge) * 0.1f;

        StartCoroutine(ImpulseRoutine(recoilDir, recoilStrength, recoilDuration));
    }

    IEnumerator ImpulseRoutine(Vector2 dir, float strength, float duration)
    {
        PlayerManager.Instance.AllowForceChange = true;
        rb.linearVelocity = Vector2.zero;
        rb.AddForce(dir * strength, ForceMode2D.Impulse);

        yield return new WaitForSeconds(duration);
        PlayerManager.Instance.AllowForceChange = false;
    }

    void CancelShot()
    {
        ResetState();
        ResetCharge();
    }

    void ResetAfterFire()
    {
        ResetState();
        ResetCharge();
        ToggleArrowSprites(3, false);
    }

    void ResetState()
    {
        IsAttacking = false;
        IsNormalAiming = false;
        IsAbilityAiming = false;
        canFire = true;
    }

    void ResetCharge()
    {
        charge = 0f;
        bowPowerSlider.value = 0f;
    }

    // ---------------- UI ----------------
    void InitUI()
    {
        bowPowerSlider.value = 0f;
        bowPowerSlider.maxValue = maxCharge;
        crossHair.SetActive(false);
    }

    void SetUIActive(bool active)
    {
        ChargeSliderUI.SetActive(active);
        if (crossHair) crossHair.SetActive(active);
    }

    void ToggleArrowSprites(int count, bool active)
    {
        for (int i = 0; i < arrowSprites.Length; i++)
            arrowSprites[i].SetActive(active && i < count);
    }
}
