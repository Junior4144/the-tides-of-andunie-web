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

    private bool isBusy = false;
    public bool IsBusy => isBusy;


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
        if (isBusy)
        {
            Debug.Log("Weapon switch ignored: currently busy");
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
        isBusy = value;
    }

    public WeaponType GetCurrentWeapon()
    {
        return currentWeapon;
    }
}
