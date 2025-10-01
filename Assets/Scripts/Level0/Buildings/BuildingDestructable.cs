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
        GameObject explosionInstance = Instantiate(explosion, transform.position, Quaternion.identity);
        AudioSource audioSource = explosionInstance.GetComponent<AudioSource>();

        if (audioSource != null && explosionSound != null)
        {
            audioSource.PlayOneShot(explosionSound, 5.0f);
        }
    }
    
}
