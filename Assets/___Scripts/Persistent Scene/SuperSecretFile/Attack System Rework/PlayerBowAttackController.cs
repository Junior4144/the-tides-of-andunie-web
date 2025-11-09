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
    [SerializeField] private float _arrowSpeedMultiplier = 25f;
    [SerializeField] private float _minArrowSpeed = 25f;
    [SerializeField] public float maxCharge = 3f;
    [SerializeField] private float _chargeRate = 3f;
    [SerializeField] private float _chargeDecreaseRate = 10f;
    [SerializeField] private float _minChargeRequired = 1;
    [SerializeField] private float _arrowSpawnOffset = 1f;
    [SerializeField] private float _minImpulseForce = 30f;
    [SerializeField] private float _maxImpulseForce = 100f;
    [SerializeField] private float _impulseDuration = 0.2f;

    public bool IsNormalAiming { get; private set; }
    public bool IsAbilityAiming { get; private set; }
    public bool IsAttacking { get; private set; }
    AudioSource _audioSource;
    Rigidbody2D rb;
    bool canFire = true;
    [HideInInspector] public float charge;
    private PlayerSquadImpulseController _impulseController;

    void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        rb = playerRoot.GetComponent<Rigidbody2D>();
        _impulseController = GetComponentInParent<PlayerSquadImpulseController>();
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
        charge = Mathf.Min(charge + Time.deltaTime * _chargeRate, maxCharge);
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

        if (charge < _minChargeRequired)
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
        ApplyImpulse();
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

        ApplyImpulse();
    }

    // ---------------- HELPERS ----------------
    void SpawnArrow(float angle, float power)
    {
        float speed = Mathf.Max(_minArrowSpeed, power * _arrowSpeedMultiplier);
        Quaternion rot = Quaternion.Euler(0f, 0f, angle);
        Vector3 spawnPos = transform.position + rot * Vector3.down * _arrowSpawnOffset;

        var arrow = Instantiate(arrowPrefab, spawnPos, rot).GetComponent<ArrowProjectile>();
        arrow.ArrowVelocity = speed;
        arrow.power = power;
    }

    void HandleCooldown()
    {
        charge = Mathf.Max(0f, charge - _chargeDecreaseRate * Time.deltaTime);
        bowPowerSlider.value = charge;
        if (charge == 0f) canFire = true;
    }

    void ApplyImpulse()
    {
        float recoilStrength = Mathf.Lerp(_minImpulseForce, _maxImpulseForce, charge / maxCharge);

        _impulseController.InitiateSquadImpulse(recoilStrength, _impulseDuration, transform.position, -rb.transform.up);
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
