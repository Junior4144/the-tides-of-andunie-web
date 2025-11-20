using System.Collections.Generic;
using UnityEngine;

public class ExplosionDamageController : MonoBehaviour
{
    private readonly HashSet<GameObject> hitEnemies = new();

    public float Power = 0;
    public float MaxPower = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject target = collision.gameObject;

        if (hitEnemies.Contains(target))
        {
            Debug.Log($"[ExplosionDamageController] Already hit {target.name}");
            return;
        }

        if (collision.TryGetComponent(out HealthController health))
        {
            hitEnemies.Add(target);

            float chargeMultiplier = Power / MaxPower;
            float finalDamage = PlayerStatsManager.Instance.DefaultExplosionDamage * chargeMultiplier;

            Debug.Log($"[ExplosionDamageController] Damage dealt {finalDamage} (base: {PlayerStatsManager.Instance.DefaultExplosionDamage}, multiplier: {chargeMultiplier})");
            health.TakeDamage(finalDamage, DamageType.Explosion);
        }

    }

}
