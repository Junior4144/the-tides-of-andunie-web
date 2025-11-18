using System.Collections.Generic;
using UnityEngine;

public class EnemyBombExplosionDamageController : MonoBehaviour
{
    [SerializeField] private PirateAttributes pirateAttribute;

    private readonly HashSet<GameObject> hitEnemies = new();
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject target = collision.gameObject;

        if (hitEnemies.Contains(target))
        {
            Debug.Log($"[ExplosionDamageController] Already hit {target.name}");
            return;
        }

        if (collision.TryGetComponent(out IHealthController health))
        {
            hitEnemies.Add(target);
            Debug.Log($"[ExplosionDamageController] Damage dealt {pirateAttribute.DamageAmount}");
            health.TakeDamage(pirateAttribute.DamageAmount);
        }

    }

}
