using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterFinal : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform target;

    [SerializeField] private float shootRate;
    [SerializeField] private float projectileMaxMoveSpeed;
    [SerializeField] private float projectileMaxHeight;

    [SerializeField] private AnimationCurve trajectoryAnimationCurve;
    [SerializeField] private AnimationCurve axisCorrectionAnimationCurve;
    [SerializeField] private AnimationCurve projectileSpeedAnimationCurve;

    private float shootTimer;

    private void Update() {
        Debug.Log($"timer:{shootTimer}");
        shootTimer -= Time.deltaTime;

        if(shootTimer <= 0 ) {
            Debug.Log("shooting");
            shootTimer = shootRate;
            Projectile projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity).GetComponent<Projectile>();

            projectile.InitializeProjectile(target, projectileMaxMoveSpeed, projectileMaxHeight);
            projectile.InitializeAnimationCurves(trajectoryAnimationCurve, axisCorrectionAnimationCurve, projectileSpeedAnimationCurve);
        }
    }
}
