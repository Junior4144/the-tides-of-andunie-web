using UnityEngine;

public class BuildingDestructable : MonoBehaviour
{
    public GameObject explosion;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("CannonBall")){
            HandleExplosion();
        }
    }

    void HandleExplosion()
    {
        Instantiate(explosion, transform.position, Quaternion.identity);
    }
    
}
