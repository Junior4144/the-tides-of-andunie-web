using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

public class ExplosionDamageController : MonoBehaviour
{
    private readonly HashSet<GameObject> hitEnemies = new();
    private CinemachineImpulseSource _impulseSource;

    public float Power = 0;
    public float MaxPower = 1;

    private void Awake()
    {
        _impulseSource = GetComponent<CinemachineImpulseSource>();

    }
    private void Start()
    {
        _impulseSource.GenerateImpulseWithForce(2f);
    }

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
            float finalDamage = PlayerStatsManager.Instance.ExplosionDamage * chargeMultiplier;

            Debug.Log($"[ExplosionDamageController] Damage dealt {finalDamage} (base: {PlayerStatsManager.Instance.ExplosionDamage}, multiplier: {chargeMultiplier}, total: {finalDamage})");

            if (collision.gameObject.CompareTag("Player"))
            {
                health.TakeDamage(finalDamage * 0.25f, DamageType.Explosion);
            }
            else
            {
                health.TakeDamage(finalDamage, DamageType.Explosion);
            } 
        }

    }

}
