using UnityEngine;

public class BuildingDestructable : MonoBehaviour
{
    public GameObject explosion;
    public AudioClip explosionSound;
    public GameObject fire;
    public bool hasExploded = false;
    Camera _camera;


    private void LateUpdate()
    {
        _camera = Camera.main;
    }


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
        if (!hasExploded)
        {
            Vector2 screenPosition = _camera.WorldToScreenPoint(transform.position);

            if (screenPosition.x >= 0 &&
                screenPosition.x <= _camera.pixelWidth &&
                screenPosition.y >= 0 &&
                screenPosition.y <= _camera.pixelHeight)
            {
                Instantiate(explosion, transform.position, Quaternion.identity);
                hasExploded = true;
                Instantiate(fire, transform.position, Quaternion.identity);
            }
        }

        if (!explosionSound)
        {
            Debug.LogWarning("Explosion sound null. Playing no sound effect.");
            return;
        }

        AudioSource.PlayClipAtPoint(explosionSound, transform.position);
    }
    
}
