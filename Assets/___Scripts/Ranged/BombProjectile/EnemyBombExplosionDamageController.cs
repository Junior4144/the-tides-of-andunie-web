using System.Collections.Generic;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyBombExplosionDamageController : MonoBehaviour
{
    [SerializeField] private PirateAttributes pirateAttribute;

    private CinemachineImpulseSource _impulseSource;
    private bool playerHit = false;

    private void Awake()
    {
        _impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (playerHit) return;

        if (collision.TryGetComponent(out PlayerHealthController playerHealth))
        {
            playerHit = true;
            Debug.Log($"Explosion dealt {pirateAttribute.DamageAmount}");
            playerHealth.TakeDamage(pirateAttribute.DamageAmount, DamageType.Explosion);

            _impulseSource.GenerateImpulseWithForce(2f);

            // Disable collider so no more triggers happen
            GetComponent<Collider2D>().enabled = false;
        }
    }

}
