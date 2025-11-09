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
    private void Start()
    {
        AxeCoolDown.SetActive(false);
        BowCoolDown.SetActive(false);
    }

    private void HandleAbilityRequest(WeaponType weaponType)
    {
        if (weaponType == WeaponType.Axe){
            AxeCoolDown.SetActive(true);
            AxeCoolDown.GetComponent<AbilityCooldownController>().ActivateAbility();}
        else if (weaponType == WeaponType.Bow){
            BowCoolDown.SetActive(true);
            BowCoolDown.GetComponent<AbilityCooldownController>().ActivateAbility();}
    }
}
