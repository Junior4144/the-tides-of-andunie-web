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

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        MaxHealth = _defaultMaxHealth;
        MeleeDamage = _defaultMeleeDamage;
    }

    public void SetMeleeDamage(float newMeleeDamage)
    {
        MeleeDamage = newMeleeDamage;
        OnDamageChanged?.Invoke(newMeleeDamage);
    }

    public void SetMaxHealth(float newMaxHealth) => MaxHealth = newMaxHealth;
}
