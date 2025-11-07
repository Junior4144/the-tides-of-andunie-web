using UnityEngine;
using System;

public class PlayerStatsManager : MonoBehaviour
{

    public static PlayerStatsManager Instance { get; private set; }

    public float MaxHealth { get; private set; }
    public float MeleeDamage { get; private set; }
    public float ExplosionDamage { get; private set; }

    [SerializeField] private float _defaultMaxHealth;
    [SerializeField] private float _defaultMeleeDamage;
    [SerializeField] private float _defaultExplosionDamage;

    public float DefaultMaxHealth => _defaultMaxHealth;
    public float DefaultMeleeDamage => _defaultMeleeDamage;
    public float DefaultExplosionDamage => _defaultExplosionDamage;

    public static event Action<float, float> OnDamageChanged;
    public static event Action<float, float> OnMaxHealthChanged;
    public static event Action<float, float> OnExplosionChanged;

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
        OnDamageChanged?.Invoke(newMeleeDamage, _defaultMeleeDamage);
    }

    public void SetMaxHealth(float newMaxHealth)
    {
        MaxHealth = newMaxHealth;
        OnMaxHealthChanged?.Invoke(newMaxHealth, _defaultMaxHealth);
    }

    public void ResetToDefaults()
    {
        MaxHealth = _defaultMaxHealth;
        MeleeDamage = _defaultMeleeDamage;

        OnMaxHealthChanged?.Invoke(_defaultMaxHealth, _defaultMaxHealth);
        OnDamageChanged?.Invoke(_defaultMeleeDamage, _defaultMeleeDamage);
    }
}
