using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public interface IHealthController
{
    void TakeDamage(float amount);
    void AddHealth(float amount);
    void SetCurrentHealth(float currentHealth);
    float GetCurrentHealth();
    float GetMaxHealth();
    float GetPercentHealth();

}