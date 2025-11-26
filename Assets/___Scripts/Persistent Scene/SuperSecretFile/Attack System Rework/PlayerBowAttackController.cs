using System.Collections;
using UnityEngine;

public class PlayerBowAttackController : MonoBehaviour
{

    [Header("References")]
    [SerializeField] Transform playerRoot;
    [SerializeField] PlayerAnimator animator;
    [SerializeField] AudioClip attackSound;
    [SerializeField] GameObject arrowPrefab;
    [SerializeField] GameObject[] arrowSprites;
    private WeaponCooldownHandler _cooldownHandler;

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
    private ImpulseController _impulseController;


    private bool isChargingAnimPlayed = false;
    private bool isChargeIdlePlayed = false;


    void Awake()
    {
        _cooldownHandler = GetComponentInParent<WeaponCooldownHandler>();
        _audioSource = GetComponent<AudioSource>();
        rb = playerRoot.GetComponent<Rigidbody2D>();
        _impulseController = GetComponentInParent<ImpulseController>();
        InitUI();
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        ToggleArrowSprites(3, false);
        ResetCharge();
        CancelShot();
        ResetState();
        animator.ReturnToDefaultIdle();
    }
    private void OnEnable()
    {
        animator.PlayBowHandleIdle();
    }

    // ---------------- UPDATE ----------------
    void Update()
    {
        // Left-Click Attack
        if (!canFire) { HandleCooldown(); return; }

        if (Input.GetMouseButton(0)) HandleAiming(normal: true);
        else if (Input.GetMouseButtonUp(0)) Fire(isAbility: false);


        if (_cooldownHandler.IsAbilityOnCooldown)
            return;

        if (Input.GetMouseButton(1))
        {
            HandleAiming(normal: false);
        }
        else if (Input.GetMouseButtonUp(1))
        {
            Fire(isAbility: true);
        }
    }

    // ---------------- CHARGING ----------------
    void HandleAiming(bool normal)
    {
        IsNormalAiming = normal;
        WeaponManager.Instance.IsNormalAiming = normal;

        IsAbilityAiming = !normal;
        WeaponManager.Instance.IsAbilityAiming = !normal;

        IsAttacking = true;
        WeaponManager.Instance.SetBusy(true);

        ToggleArrowSprites(normal ? 1 : 3, true);
        charge = Mathf.Min(charge + Time.deltaTime * _chargeRate, maxCharge);
        BowPowerUIManager.instance.slider.value = charge;
        WeaponManager.Instance.CurrentBowCharge = charge;

        if (!isChargingAnimPlayed)
        {
            isChargingAnimPlayed = true;
            isChargeIdlePlayed = false;
            animator.PlayBowCharge();  // play 0.45s draw animation
            StartCoroutine(TransitionToChargeIdleAfter(0.45f)); // hold pose after
        }
    }

    IEnumerator TransitionToChargeIdleAfter(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (IsAttacking && !isChargeIdlePlayed)
        {
            isChargeIdlePlayed = true;
            animator.PlayBowChargeIdle();  // now holding the bow drawn
        }
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
        WeaponManager.Instance.CurrentBowCharge = charge;

        if (isAbility)
        {
            WeaponEvents.OnWeaponAbilityActivation?.Invoke(WeaponType.Bow);
            _cooldownHandler.StartAbilityCooldown();
            FireSpreadShot();
        }
        else FireSingleShot();

        animator.PlayBowHandleIdle(); // ← return to idle bow pose after firing
        ResetAfterFire();
        isChargingAnimPlayed = false;
        isChargeIdlePlayed = false;
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
        arrow.maxPower = maxCharge;
    }

    void HandleCooldown()
    {
        charge = Mathf.Max(0f, charge - _chargeDecreaseRate * Time.deltaTime);
        BowPowerUIManager.instance.slider.value = charge;
        WeaponManager.Instance.CurrentBowCharge = charge;

        if (charge == 0f) canFire = true;
    }

    void ApplyImpulse()
    {
        float recoilStrength = Mathf.Lerp(_minImpulseForce, _maxImpulseForce, charge / maxCharge);

        var impulseSettings = new ImpulseSettings
        {
            Force = recoilStrength,
            Duration = _impulseDuration,
            PlaySound = true,
            SpawnParticles = true
        };

        _impulseController.InitiateSquadImpulse(transform.position, -rb.transform.up, impulseSettings);
    }


    void CancelShot()
    {
        animator.PlayBowHandleIdle(); // ← return to holding bow pose
        ResetState();
        ResetCharge();
        ToggleArrowSprites(3, false);
        isChargingAnimPlayed = false;
        isChargeIdlePlayed = false;
    }


    void ResetAfterFire()
    {
        animator.PlayBowHandleIdle();
        ResetState();
        ResetCharge();
        ToggleArrowSprites(3, false);
        isChargingAnimPlayed = false;
        isChargeIdlePlayed = false;
    }

    void ResetState()
    {
        IsAttacking = false;
        WeaponManager.Instance.SetBusy(false);

        IsNormalAiming = false;
        WeaponManager.Instance.IsNormalAiming = false;

        IsAbilityAiming = false;
        WeaponManager.Instance.IsAbilityAiming = false;

        canFire = true;
    }

    void ResetCharge()
    {
        charge = 0f;
        WeaponManager.Instance.CurrentBowCharge = charge;
        BowPowerUIManager.instance.slider.value = 0f;
    }

    // ---------------- UI ----------------
    void InitUI()
    {
        BowPowerUIManager.instance.slider.value = 0f;
        BowPowerUIManager.instance.slider.maxValue = maxCharge;
        WeaponManager.Instance.BowMaxCharge = maxCharge;
    }

    void ToggleArrowSprites(int count, bool active)
    {
        for (int i = 0; i < arrowSprites.Length; i++)
            arrowSprites[i].SetActive(active && i < count);
    }
}
