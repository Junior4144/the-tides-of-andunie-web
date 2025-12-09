using UnityEngine;

public class PlayerWeaponSwitcher : MonoBehaviour
{

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            WeaponEvents.OnEquipWeaponRequest?.Invoke(WeaponType.Axe);

        if (Input.GetKeyDown(KeyCode.Alpha2))
            WeaponEvents.OnEquipWeaponRequest?.Invoke(WeaponType.Bow);

    }
    
}
