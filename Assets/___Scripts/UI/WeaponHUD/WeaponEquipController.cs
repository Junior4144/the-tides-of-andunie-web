using System.Collections.Generic;
using UnityEngine;

public class WeaponEuipController : MonoBehaviour
{
    [SerializeField] private WeaponType myType;
    [SerializeField] private List<GameObject> weapons;

    private void OnEnable()
    {
        WeaponEvents.OnNewWeaponEquipped += HandleEquipRequest;
    }

    private void OnDisable()
    {
        WeaponEvents.OnNewWeaponEquipped -= HandleEquipRequest;
    }

    private void HandleEquipRequest(WeaponType equippedType)
    {
        bool isMyWeapon = equippedType == myType;

        weapons.ForEach(weapon => weapon.SetActive(isMyWeapon));

        if (isMyWeapon)
            Debug.Log($"{myType} activated");
    }
}
