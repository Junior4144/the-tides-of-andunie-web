using UnityEngine;

public class BowWeapon : MonoBehaviour
{
    public GameObject bullet;

    public Transform firePoint;

    public float fireForce;

    public void Fire()
    {
        GameObject projectile = Instantiate(bullet, firePoint.position, firePoint.rotation);
        projectile.GetComponent<Rigidbody2D>().AddForce(firePoint.up * fireForce, ForceMode2D.Impulse);

    }
}
