using UnityEngine;

public class WeaponHUDController : MonoBehaviour
{
    [SerializeField] private GameObject AxeCoolDown;
    [SerializeField] private GameObject BowCoolDown;

    private void OnEnable()
    {
        WeaponEvents.OnWeaponAbilityActivation += HandleAbilityRequest;
    }

    private void OnDisable()
    {
        WeaponEvents.OnWeaponAbilityActivation -= HandleAbilityRequest;
    }

    private void HandleAbilityRequest(WeaponType weaponType)
    {
        if (weaponType == WeaponType.Axe){
            AxeCoolDown.GetComponent<AbilityCooldownController>().ActivateAbility();}
        else if (weaponType == WeaponType.Bow){
            BowCoolDown.GetComponent<AbilityCooldownController>().ActivateAbility();}
    }
}
