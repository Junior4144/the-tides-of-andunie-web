using UnityEngine;

public class BuildingDestructable : MonoBehaviour
{
    public GameObject explosion;
    public AudioClip explosionSound;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("CannonBall")){
            HandleExplosion();
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }

    void HandleExplosion()
    {
        Instantiate(explosion, transform.position, Quaternion.identity);

        if (!explosionSound)
        {
            Debug.LogWarning("Explosion sound null. Playing no sound effect.");
            return;
        }

        AudioSource.PlayClipAtPoint(explosionSound, transform.position);
    }
    
}
