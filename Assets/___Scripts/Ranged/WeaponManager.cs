using System;
using UnityEngine;

public enum WeaponType
{
    none,
    Axe,
    Bow,
}

public static class WeaponEvents
{
    public static Action<WeaponType> OnEquipWeaponRequest; // Input asks for this
    public static Action<WeaponType> OnNewWeaponEquipped;     // Broadcasted once equipped
    public static Action<WeaponType> OnWeaponAbilityActivation;
}

public class WeaponManager : MonoBehaviour
{
    public static WeaponManager Instance { get; private set; }

    [SerializeField] private WeaponType currentWeapon = WeaponType.none;
    private WeaponType? pendingWeaponRequest = null;

    public bool IsBusy { get; private set; } = false;

    public float CurrentBowCharge;
    public float BowMaxCharge;
    public bool IsNormalAiming;
    public bool IsAbilityAiming;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void OnEnable()
    {
        WeaponEvents.OnEquipWeaponRequest += HandleEquipRequest;
    }

    private void OnDisable()
    {
        WeaponEvents.OnEquipWeaponRequest -= HandleEquipRequest;
    }

    private void Start()
    {
        HandleEquipRequest(WeaponType.Axe);
    }

    private void HandleEquipRequest(WeaponType requestedWeapon)
    {
        if (IsBusy)
        {
            pendingWeaponRequest = requestedWeapon;
            Debug.Log($"Weapon switch to {requestedWeapon} queued (currently busy).");
            return;
        }

        if (currentWeapon == requestedWeapon)
        {
            Debug.Log($"Weapon {requestedWeapon} already equipped.");
            return;
        }

        EquipWeapon(requestedWeapon);
    }

    private void EquipWeapon(WeaponType newWeapon)
    {
        currentWeapon = newWeapon;
        Debug.Log($"Equipped weapon: {newWeapon}");
        WeaponEvents.OnNewWeaponEquipped?.Invoke(newWeapon);
    }

    public void SetBusy(bool value)
    {
        bool wasBusy = IsBusy;
        IsBusy = value;

        if (wasBusy && !IsBusy)
        {
            if (pendingWeaponRequest.HasValue)
            {
                WeaponType queuedWeapon = pendingWeaponRequest.Value;
                pendingWeaponRequest = null;
                EquipWeapon(queuedWeapon);
            }
        }
    }

    public WeaponType GetCurrentWeapon()
    {
        return currentWeapon;
    }
}
