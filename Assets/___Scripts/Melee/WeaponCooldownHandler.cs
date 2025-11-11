using UnityEngine;

public class WeaponCooldownHandler : MonoBehaviour
{
    private bool isAbilityOnCooldown = false;
    private float abilityCooldownTimer = 0f;

    [SerializeField] private float abilityCooldownDuration = 5f;

    public bool IsAbilityOnCooldown => isAbilityOnCooldown;

    private void Update()
    {
        if (isAbilityOnCooldown)
            HandleAbilityCooldown();
    }

    public void StartAbilityCooldown()
    {
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