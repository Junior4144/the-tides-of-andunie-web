using UnityEngine;
using UnityEngine.UI;

public class WeaponHUDController : MonoBehaviour
{
    [SerializeField] private Image AxeWeaponIcon;
    [SerializeField] private Image BowWeaponIcon;

    [SerializeField] private GameObject AxeCoolDown;
    [SerializeField] private GameObject BowCoolDown;

    private void Awake()
    {
        AxeCoolDown.SetActive(false);
        BowCoolDown.SetActive(false);
    }
    
    private void OnEnable()
    {
        WeaponEvents.OnNewWeaponEquipped += HandleEquippedWeapon;
        WeaponEvents.OnWeaponAbilityActivation += HandleAbilityRequest;
    }

    private void OnDisable()
    {
        WeaponEvents.OnNewWeaponEquipped -= HandleEquippedWeapon;
        WeaponEvents.OnWeaponAbilityActivation -= HandleAbilityRequest;
    }

    private void HandleEquippedWeapon(WeaponType equippedWeapon)
    {
        SetWeaponTransparency(AxeWeaponIcon, 0.3f);
        SetWeaponTransparency(BowWeaponIcon, 0.3f);

        switch (equippedWeapon)
        {
            case WeaponType.Axe:
                SetWeaponTransparency(AxeWeaponIcon, 1f);
                break;

            case WeaponType.Bow:
                SetWeaponTransparency(BowWeaponIcon, 1f);
                break;
        }
    }

    private void SetWeaponTransparency(Image icon, float alpha)
    {
        Color color = icon.color;
        color.a = alpha;
        icon.color = color;
    }

    private void HandleAbilityRequest(WeaponType weaponType, float cooldownDuration)
    {
        if (weaponType == WeaponType.Axe){
            AxeCoolDown.SetActive(true);
            AxeCoolDown.GetComponent<AbilityCooldownController>().ActivateAbility(cooldownDuration);}
        else if (weaponType == WeaponType.Bow){
            BowCoolDown.SetActive(true);
            BowCoolDown.GetComponent<AbilityCooldownController>().ActivateAbility(cooldownDuration);}
    }
}
