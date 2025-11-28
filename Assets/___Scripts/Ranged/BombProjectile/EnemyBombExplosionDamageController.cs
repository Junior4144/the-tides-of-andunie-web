using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyBombExplosionDamageController : MonoBehaviour
{
    [SerializeField] private PirateAttributes pirateAttribute;

    private bool playerHit = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (playerHit) return;

        if (collision.TryGetComponent(out PlayerHealthController playerHealth))
        {
            playerHit = true;
            Debug.Log($"Explosion dealt {pirateAttribute.DamageAmount}");
            playerHealth.TakeDamage(pirateAttribute.DamageAmount, DamageType.Explosion);

            // Disable collider so no more triggers happen
            GetComponent<Collider2D>().enabled = false;
        }
    }

}
