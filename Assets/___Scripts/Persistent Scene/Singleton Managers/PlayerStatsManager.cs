using UnityEngine;
using System;

public class PlayerStatsManager : MonoBehaviour
{

    public static PlayerStatsManager Instance { get; private set; }

    public float MaxHealth { get; private set; }
    public float MeleeDamage { get; private set; }

    [SerializeField] private float _defaultMaxHealth;
    [SerializeField] private float _defaultMeleeDamage;

    public static event Action<float> OnDamageChanged;
    public static event Action<float> OnMaxHealthChanged;

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
        OnDamageChanged?.Invoke(newMeleeDamage);
    }

    public void SetMaxHealth(float newMaxHealth)
    {
        MaxHealth = newMaxHealth;
        OnMaxHealthChanged?.Invoke(newMaxHealth);
    }

    public void ResetToDefaults()
    {
        MaxHealth = _defaultMaxHealth;
        MeleeDamage = _defaultMeleeDamage;

        OnMaxHealthChanged?.Invoke(_defaultMaxHealth);
        OnDamageChanged?.Invoke(_defaultMeleeDamage);
    }
}
