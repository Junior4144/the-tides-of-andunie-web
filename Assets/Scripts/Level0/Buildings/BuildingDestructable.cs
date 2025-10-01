using UnityEngine;

public class BuildingDestructable : MonoBehaviour
{
    public GameObject explosion;
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

    }
    
}
