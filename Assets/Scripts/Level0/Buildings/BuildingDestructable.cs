using UnityEngine;

public class BuildingDestructable : MonoBehaviour
{
    public GameObject explosion;
    public bool hasExploded = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("CannonBall")){
            HandleExplosion();
        }
    }

    void HandleExplosion()
    {
        if (!hasExploded)
        {
            Instantiate(explosion, transform.position, Quaternion.identity);
            hasExploded = true;
        }
        
    }
    
}
