using UnityEngine;

public class BuildingDestructable : MonoBehaviour
{
    public GameObject explosion;
    public GameObject fire;
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
            Instantiate(fire, transform.position, Quaternion.identity);
        }
        
    }
    
}
