using UnityEngine;
using System;

public class PlayerStatsManager : MonoBehaviour
{
    // ========================================
    // HOW TO ADD A NEW STAT
    // ========================================
    // Follow these steps to add a new stat. The naming convention is CRITICAL
    // as other scripts use reflection to automatically discover stats.
    //
    // Example: Adding a stat called "CriticalChance"
    //
    // 1. Add to StatType enum (in StatType.cs):
    //    [StatDisplay("Critical Chance")]
    //    CriticalChance
    //
    // 2. Add public property (MUST match StatType name exactly):
    //    public float CriticalChance { get; private set; }
    //
    // 3. Add serialized default field (prefix with underscore and lowercase first letter):
    //    [SerializeField] private float _defaultCriticalChance;
    //
    // 4. Add default property (prefix with "Default" + StatType name):
    //    public float DefaultCriticalChance => _defaultCriticalChance;
    //
    // 5. Add event (prefix with "On" + StatType name + "Changed"):
    //    public static event Action<float, float> OnCriticalChanceChanged;
    //
    // 6. Add setter method (prefix with "Set" + StatType name):
    //    public void SetCriticalChance(float newCriticalChance)
    //    {
    //        CriticalChance = newCriticalChance;
    //        OnCriticalChanceChanged?.Invoke(newCriticalChance, _defaultCriticalChance);
    //    }
    //
    // 7. Initialize in ResetToDefaults():
    //    CriticalChance = _defaultCriticalChance;
    //    OnCriticalChanceChanged?.Invoke(_defaultCriticalChance, _defaultCriticalChance);
    //
    // That's it! The UI will automatically display the new stat.
    // ========================================

    public static PlayerStatsManager Instance { get; private set; }

    public float MaxHealth { get; private set; }
    public float MeleeDamage { get; private set; }
    public float ExplosionDamage { get; private set; }
    public float Speed { get; private set; }

    [SerializeField] private float _defaultMaxHealth;
    [SerializeField] private float _defaultMeleeDamage;
    [SerializeField] private float _defaultExplosionDamage;
    [SerializeField] private float _defaultSpeed;
    
    public float DefaultMaxHealth => _defaultMaxHealth;
    public float DefaultMeleeDamage => _defaultMeleeDamage;
    public float DefaultExplosionDamage => _defaultExplosionDamage;
    public float DefaultSpeed => _defaultSpeed;

    public static event Action<float, float> OnMeleeDamageChanged;
    public static event Action<float, float> OnMaxHealthChanged;
    public static event Action<float, float> OnExplosionDamageChanged;
    public static event Action<float, float> OnSpeedChanged;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        ResetToDefaults();
    }

    public void SetMeleeDamage(float newMeleeDamage)
    {
        MeleeDamage = newMeleeDamage;
        OnMeleeDamageChanged?.Invoke(newMeleeDamage, _defaultMeleeDamage);
    }

    public void SetMaxHealth(float newMaxHealth)
    {
        MaxHealth = newMaxHealth;
        OnMaxHealthChanged?.Invoke(newMaxHealth, _defaultMaxHealth);
    }

    public void SetExplosionDamage(float newExplosionDamage)
    {
        ExplosionDamage = newExplosionDamage;
        OnExplosionDamageChanged?.Invoke(newExplosionDamage, _defaultExplosionDamage);
    }

    public void SetSpeed(float newSpeed)
    {
        Speed = newSpeed;
        OnSpeedChanged?.Invoke(newSpeed, _defaultSpeed);
    }

    public void ResetToDefaults()
    {
        MaxHealth = _defaultMaxHealth;
        MeleeDamage = _defaultMeleeDamage;
        ExplosionDamage = _defaultExplosionDamage;
        Speed = _defaultSpeed;

        OnMaxHealthChanged?.Invoke(_defaultMaxHealth, _defaultMaxHealth);
        OnMeleeDamageChanged?.Invoke(_defaultMeleeDamage, _defaultMeleeDamage);
        OnExplosionDamageChanged?.Invoke(_defaultExplosionDamage, _defaultExplosionDamage);
        OnSpeedChanged?.Invoke(_defaultSpeed, _defaultSpeed);
    }
}
