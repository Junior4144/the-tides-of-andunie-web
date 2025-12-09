using UnityEngine;

public class CrosshairUIController : MonoBehaviour
{
    [SerializeField] GameObject crosshairUI;
    [SerializeField] GameObject BowPowerUI;

    private void OnEnable()
    {
        WeaponEvents.OnNewWeaponEquipped += HandleCrossHairUiActivation;
    }
    private void OnDisable()
    {
        WeaponEvents.OnNewWeaponEquipped -= HandleCrossHairUiActivation;
    }

    private void HandleCrossHairUiActivation(WeaponType weaponType)
    {
        if(weaponType == WeaponType.Bow)
        {
            crosshairUI.SetActive(true);
            BowPowerUI.SetActive(true);
        }
        else
        {
            crosshairUI.SetActive(false);
            BowPowerUI.SetActive(false);
        }
    }
}
