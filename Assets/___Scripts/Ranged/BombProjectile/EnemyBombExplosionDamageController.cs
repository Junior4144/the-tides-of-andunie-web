using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyBombExplosionDamageController : MonoBehaviour
{
    [SerializeField] private PirateAttributes pirateAttribute;

    private readonly HashSet<GameObject> hitEnemies = new();

    private bool playerHit = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject target = collision.gameObject;

        if (hitEnemies.Contains(target))
        {
            return;
        }

        if (collision.TryGetComponent(out PlayerHealthController playerHealth) && !playerHit)
        {
            playerHit = true;
            hitEnemies.Add(target);
            Debug.Log($"[ExplosionDamageController] Damage dealt {pirateAttribute.DamageAmount}");
            playerHealth.TakeDamage(pirateAttribute.DamageAmount);
        }

    }

}
