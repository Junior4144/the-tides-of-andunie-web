using UnityEngine;

public class CrosshairUIController : MonoBehaviour
{
    [SerializeField] GameObject crosshairUI;

    private void OnEnable()
    {
        WeaponEvents.OnEquipWeaponRequest += HandleCrossHairUiActivation;
    }
    private void OnDisable()
    {
        WeaponEvents.OnEquipWeaponRequest += HandleCrossHairUiActivation;
    }

    private void HandleCrossHairUiActivation(WeaponType weaponType)
    {
        if(weaponType == WeaponType.Bow)
        {
            crosshairUI.SetActive(true);
        }
        else
        {
            crosshairUI.SetActive(false);
        }
    }
}
