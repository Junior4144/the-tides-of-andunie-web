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

    // =============================
    //  STAT PROPERTIES
    // =============================
    public float MaxHealth { get; private set; }
    public float MeleeDamage { get; private set; }
    public float ExplosionDamage { get; private set; }
    public float Speed { get; private set; }

    public float MeleeResistance { get; private set; }
    public float RangedResistance { get; private set; }
    public float ExplosionResistance { get; private set; }

    // =============================
    //  DEFAULT VALUES (serialized)
    // =============================
    [SerializeField] private float _defaultMaxHealth;
    [SerializeField] private float _defaultMeleeDamage;
    [SerializeField] private float _defaultExplosionDamage;
    [SerializeField] private float _defaultSpeed;

    [SerializeField] private float _defaultMeleeResistance;
    [SerializeField] private float _defaultRangedResistance;
    [SerializeField] private float _defaultExplosionResistance;

    // =============================
    //  DEFAULT GETTERS
    // =============================
    public float DefaultMaxHealth => _defaultMaxHealth;
    public float DefaultMeleeDamage => _defaultMeleeDamage;
    public float DefaultExplosionDamage => _defaultExplosionDamage;
    public float DefaultSpeed => _defaultSpeed;

    public float DefaultMeleeResistance => _defaultMeleeResistance;
    public float DefaultRangedResistance => _defaultRangedResistance;
    public float DefaultExplosionResistance => _defaultExplosionResistance;

    // =============================
    //  EVENTS
    // =============================
    public static event Action<float, float> OnMaxHealthChanged;
    public static event Action<float, float> OnMeleeDamageChanged;
    public static event Action<float, float> OnExplosionDamageChanged;
    public static event Action<float, float> OnSpeedChanged;

    public static event Action<float, float> OnMeleeResistanceChanged;
    public static event Action<float, float> OnRangedResistanceChanged;
    public static event Action<float, float> OnExplosionResistanceChanged;

    // =============================
    //  UNITY LIFECYCLE
    // =============================
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        ResetToDefaults();
    }

    // =============================
    //  SETTERS
    // =============================
    public void SetMaxHealth(float value)
    {
        MaxHealth = value;
        OnMaxHealthChanged?.Invoke(value, _defaultMaxHealth);
    }

    public void SetMeleeDamage(float value)
    {
        MeleeDamage = value;
        OnMeleeDamageChanged?.Invoke(value, _defaultMeleeDamage);
    }

    public void SetExplosionDamage(float value)
    {
        ExplosionDamage = value;
        OnExplosionDamageChanged?.Invoke(value, _defaultExplosionDamage);
    }

    public void SetSpeed(float value)
    {
        Speed = value;
        OnSpeedChanged?.Invoke(value, _defaultSpeed);
    }

    public void SetMeleeResistance(float value)
    {
        MeleeResistance = value;
        OnMeleeResistanceChanged?.Invoke(value, _defaultMeleeResistance);
    }

    public void SetRangedResistance(float value)
    {
        RangedResistance = value;
        OnRangedResistanceChanged?.Invoke(value, _defaultRangedResistance);
    }

    public void SetExplosionResistance(float value)
    {
        ExplosionResistance = value;
        OnExplosionResistanceChanged?.Invoke(value, _defaultExplosionResistance);
    }

    // =============================
    //  RESETTER
    // =============================
    public void ResetToDefaults()
    {
        MaxHealth = _defaultMaxHealth;
        MeleeDamage = _defaultMeleeDamage;
        ExplosionDamage = _defaultExplosionDamage;
        Speed = _defaultSpeed;

        MeleeResistance = _defaultMeleeResistance;
        RangedResistance = _defaultRangedResistance;
        ExplosionResistance = _defaultExplosionResistance;

        OnMaxHealthChanged?.Invoke(_defaultMaxHealth, _defaultMaxHealth);
        OnMeleeDamageChanged?.Invoke(_defaultMeleeDamage, _defaultMeleeDamage);
        OnExplosionDamageChanged?.Invoke(_defaultExplosionDamage, _defaultExplosionDamage);
        OnSpeedChanged?.Invoke(_defaultSpeed, _defaultSpeed);

        OnMeleeResistanceChanged?.Invoke(_defaultMeleeResistance, _defaultMeleeResistance);
        OnRangedResistanceChanged?.Invoke(_defaultRangedResistance, _defaultRangedResistance);
        OnExplosionResistanceChanged?.Invoke(_defaultExplosionResistance, _defaultExplosionResistance);
    }
}
