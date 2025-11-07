using UnityEngine;

public class PlayerWeaponSwitcher : MonoBehaviour
{
    [SerializeField] private GameObject[] weapons;
    private int currentWeaponIndex = 0;

    void Start()
    {
        // Deactivate all weapons first
        for (int i = 0; i < weapons.Length; i++)
            weapons[i].SetActive(false);

        // Activate the first weapon if available
        if (weapons.Length > 0)
            weapons[0].SetActive(true);
    }

    void Update()
    {
        // Check number keys (1, 2, 3...) based on weapon count
        for (int i = 0; i < weapons.Length; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                SwitchWeapon(i);
            }
        }
    }

    private void SwitchWeapon(int index)
    {
        if (index == currentWeaponIndex || index < 0 || index >= weapons.Length)
            return;

        // Deactivate current weapon
        weapons[currentWeaponIndex].SetActive(false);

        // Activate new weapon
        weapons[index].SetActive(true);
        currentWeaponIndex = index;

        Debug.Log($"Switched to: {weapons[index].name}");
    }
}
