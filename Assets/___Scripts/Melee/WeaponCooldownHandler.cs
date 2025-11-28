using UnityEngine;

public class WeaponCooldownHandler : MonoBehaviour
{
    [SerializeField] private WeaponType weaponType;
    [SerializeField] private float abilityCooldownDuration = 5f;

    private bool isAbilityOnCooldown = false;
    private float abilityCooldownTimer = 0f;

    public bool IsAbilityOnCooldown => isAbilityOnCooldown;

    private void Update()
    {
        if (isAbilityOnCooldown)
            HandleAbilityCooldown();
    }

    public void StartAbilityCooldown()
    {
        WeaponEvents.OnWeaponAbilityActivation?.Invoke(weaponType, abilityCooldownDuration);
        isAbilityOnCooldown = true;
        abilityCooldownTimer = abilityCooldownDuration;
        Debug.Log("Ability cooldown started.");
    }

    private void HandleAbilityCooldown()
    {
        abilityCooldownTimer -= Time.deltaTime;

        if (abilityCooldownTimer <= 0f)
        {
            isAbilityOnCooldown = false;
            abilityCooldownTimer = 0f;
            Debug.Log("Ability ready again!");
        }
    }
}