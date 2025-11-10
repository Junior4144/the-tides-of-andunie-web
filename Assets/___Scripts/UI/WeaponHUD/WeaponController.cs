using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [SerializeField] private WeaponType myType;
    [SerializeField] private GameObject weapon;


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

        weapon.SetActive(isMyWeapon);

        if (isMyWeapon)
            Debug.Log($"{myType} activated");
    }
}
