using UnityEngine;

public class PlayerStatsManager : MonoBehaviour
{

    public static PlayerStatsManager Instance { get; private set; }

    public static float MaxHealth;
    public static float MeleeDamage;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }


}
