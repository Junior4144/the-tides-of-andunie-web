using System.Collections.Generic;
using UnityEngine;

public class ExplosionDamageController : MonoBehaviour
{
    private readonly HashSet<Collider2D> hitEnemies = new();

    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (hitEnemies.Contains(collision)) return;

        if (collision.TryGetComponent(out IHealthController health))
        {
            hitEnemies.Add(collision);
            health.TakeDamage(PlayerStatsManager.Instance.DefaultExplosionDamage);
        }
    }

}
