using UnityEngine;
using UnityEngine.UI;

public class DynamicCrosshair : MonoBehaviour
{
    public RectTransform crosshairRoot;
    public RectTransform top, bottom, left, right;
    public PlayerBowAttackController bowAttackController;

    [Header("Settings")]
    public float idleSpread = 40f;
    public float aimSpread = 10f;
    public float abilityMaxSpread = 100f;  // Big spread when ability starts
    public float abilityMinSpread = 20f;   // Shrinks down to this while charging
    public float moveSpeed = 10f;

    [Header("Visual Effects")]
    public Color normalColor = Color.white;
    public Color abilityColor = new Color(1f, 0.4f, 0.2f); // orange-red
    public float pulseSpeed = 8f;
    public float pulseAmount = 0.05f;
    public float rotationSpeed = 100f;

    private float currentSpread;
    private Image[] crosshairParts;

    void OnEnable()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = false;
        currentSpread = idleSpread;

        crosshairParts = GetComponentsInChildren<Image>();
    }

    void OnDisable()
    {
        Cursor.visible = true;
    }

    void Update()
    {
        // Move crosshair to mouse
        crosshairRoot.position = Input.mousePosition;

        // --- Determine spread target ---
        float targetSpread = idleSpread;

        if (bowAttackController.IsNormalAiming)
        {
            targetSpread = aimSpread;
        }
        else if (bowAttackController.IsAbilityAiming)
        {
            // Spread starts big and shrinks as charge increases
            float chargeRatio = bowAttackController.charge / bowAttackController.maxCharge;
            targetSpread = Mathf.Lerp(abilityMaxSpread, abilityMinSpread, chargeRatio);
        }

        // Smooth transition
        currentSpread = Mathf.Lerp(currentSpread, targetSpread, Time.deltaTime * moveSpeed);

        // Update crosshair positions
        top.localPosition = new Vector3(0, currentSpread, 0);
        bottom.localPosition = new Vector3(0, -currentSpread, 0);
        left.localPosition = new Vector3(-currentSpread, 0, 0);
        right.localPosition = new Vector3(currentSpread, 0, 0);

        // --- Visual FX: Color + Rotation + Pulse ---
        UpdateCrosshairVisuals();
    }

    void UpdateCrosshairVisuals()
    {
        bool isAbility = bowAttackController.IsAbilityAiming;

        // 1️⃣ Color shift
        Color targetColor = isAbility ? abilityColor : normalColor;
        foreach (var img in crosshairParts)
            img.color = Color.Lerp(img.color, targetColor, Time.deltaTime * 10f);

        // 2️⃣ Pulse when in ability mode
        if (isAbility)
        {
            float pulse = Mathf.Sin(Time.time * pulseSpeed) * pulseAmount + 1f;
            crosshairRoot.localScale = Vector3.one * pulse;

            // 3️⃣ Rotate slowly for visual motion
            crosshairRoot.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
        }
        else
        {
            // Reset rotation/scale smoothly when not using ability
            crosshairRoot.localScale = Vector3.Lerp(crosshairRoot.localScale, Vector3.one, Time.deltaTime * 8f);
            crosshairRoot.localRotation = Quaternion.Lerp(crosshairRoot.localRotation, Quaternion.identity, Time.deltaTime * 8f);
        }
    }
}
