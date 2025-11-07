using UnityEngine;
using UnityEngine.UI;

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
    }
    private void OnDisable()
    {
        bowPowerSlider.value = 0f;
        charge = 0f;
        ChargeSliderUI.SetActive(false);
    }

    public bool IsAttacking => isAttacking;

    void Awake()
    {
        audioSrc = GetComponent<AudioSource>();
        rb = playerRoot.GetComponent<Rigidbody2D>();
        bowPowerSlider.value = 0f;
        bowPowerSlider.maxValue = maxCharge;
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

        var arrow = Instantiate(arrowPrefab, transform.position, rot)
            .GetComponent<ArrowProjectile>();

        arrow.ArrowVelocity = speed;
        arrow.power = charge;

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
}
