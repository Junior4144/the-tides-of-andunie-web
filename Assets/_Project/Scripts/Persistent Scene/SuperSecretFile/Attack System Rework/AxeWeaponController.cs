using UnityEngine;

public class AxeWeaponController : MonoBehaviour
{
    [Header("Attack References")]
    [SerializeField] private BaseAttack normalAttack;
    [SerializeField] private BaseAttack heavyAttack;

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !IsAnyAttackActive() && IsAttackPointActive(normalAttack) && GameManager.Instance.CurrentState == GameState.Gameplay)
            normalAttack.Execute();

        if (Input.GetMouseButtonDown(1) && !IsAnyAttackActive() && IsAttackPointActive(heavyAttack) && GameManager.Instance.CurrentState == GameState.Gameplay)
            heavyAttack.Execute();
    }

    bool IsAnyAttackActive()
    {
        bool normalActive = normalAttack != null && normalAttack.IsAttacking;
        bool heavyActive = heavyAttack != null && heavyAttack.IsAttacking;
        return normalActive || heavyActive;
    }

    bool IsAttackPointActive(BaseAttack attack) =>
        attack != null && attack.gameObject.activeInHierarchy;
}
