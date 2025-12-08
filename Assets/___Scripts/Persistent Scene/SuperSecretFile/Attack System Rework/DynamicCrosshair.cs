using UnityEngine;
using UnityEngine.UI;

public class DynamicCrosshair : MonoBehaviour
{
    [Header("References")]
    [SerializeField] RectTransform crosshairRoot;
    [SerializeField] RectTransform top, bottom, left, right;

    [Header("Spread Settings")]
    [SerializeField] float idleSpread = 40f;
    [SerializeField] float aimSpread = 10f;
    [SerializeField] float abilityMaxSpread = 100f;
    [SerializeField] float abilityMinSpread = 20f;
    [SerializeField] float moveSpeed = 10f;

    [Header("Visual Effects")]
    [SerializeField] Color normalColor = Color.white;
    [SerializeField] Color abilityColor = new(1f, 0.4f, 0.2f);
    [SerializeField] float pulseSpeed = 8f;
    [SerializeField] float pulseAmount = 0.05f;
    [SerializeField] float rotationSpeed = 100f;

    float currentSpread;
    Image[] crosshairParts;
    
    

    // ---------------- UNITY LIFECYCLE ----------------
    void OnEnable()
    {
        SetupCursor(false);
        crosshairParts = GetComponentsInChildren<Image>();
        currentSpread = idleSpread;
    }

    void OnDisable()
    {
        SetupCursor(true);
    }

    void Update()
    {
        UpdatePosition();
        UpdateSpread();
        ApplyCrosshairTransforms();
        ApplyVisualEffects();
    }

    // ---------------- CORE BEHAVIOR ----------------
    void UpdatePosition()
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            (RectTransform)crosshairRoot.parent,
            Input.mousePosition,
            null,
            out Vector2 localPos
        );

        crosshairRoot.anchoredPosition = localPos;
    }

    void UpdateSpread()
    {
        float targetSpread = GetTargetSpread();

        // If you're not aiming or charging, instantly reset
        if (!WeaponManager.Instance.IsNormalAiming && !WeaponManager.Instance.IsAbilityAiming)
            currentSpread = targetSpread;
        else
            currentSpread = Mathf.Lerp(currentSpread, targetSpread, Time.deltaTime * moveSpeed);
    }

    float GetTargetSpread()
    {
        if (WeaponManager.Instance.IsNormalAiming) return aimSpread;
        if (WeaponManager.Instance.IsAbilityAiming)
        {
            float chargeRatio = WeaponManager.Instance.CurrentBowCharge / WeaponManager.Instance.BowMaxCharge;
            return Mathf.Lerp(abilityMaxSpread, abilityMinSpread, chargeRatio);
        }
        return idleSpread;
    }

    void ApplyCrosshairTransforms()
    {
        Vector3 up = Vector3.up * currentSpread;
        Vector3 rightDir = Vector3.right * currentSpread;

        top.anchoredPosition = up;
        bottom.anchoredPosition = -up;
        left.anchoredPosition = -rightDir;
        right.anchoredPosition = rightDir;
    }

    // ---------------- VISUAL EFFECTS ----------------
    void ApplyVisualEffects()
    {
        bool isAbility = WeaponManager.Instance.IsAbilityAiming;
        UpdateColor(isAbility);
        UpdatePulseAndRotation(isAbility);
    }

    void UpdateColor(bool isAbility)
    {
        Color target = isAbility ? abilityColor : normalColor;
        foreach (var img in crosshairParts)
            img.color = Color.Lerp(img.color, target, Time.deltaTime * 10f);
    }

    void UpdatePulseAndRotation(bool isAbility)
    {
        if (isAbility)
        {
            float pulse = Mathf.Sin(Time.time * pulseSpeed) * pulseAmount + 1f;
            crosshairRoot.localScale = Vector3.one * pulse;
            crosshairRoot.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
        }
        else
        {
            crosshairRoot.localScale = Vector3.Lerp(crosshairRoot.localScale, Vector3.one, Time.deltaTime * 8f);
            crosshairRoot.localRotation = Quaternion.Lerp(crosshairRoot.localRotation, Quaternion.identity, Time.deltaTime * 8f);
        }
    }

    // ---------------- HELPERS ----------------
    void SetupCursor(bool visible)
    {
        Cursor.visible = visible;
        Cursor.lockState = visible ? CursorLockMode.None : CursorLockMode.Confined;
    }
}
