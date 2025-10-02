using UnityEngine;

public abstract class BuildingDestructable : MonoBehaviour
{
    public GameObject explosion;
    public GameObject fire;
    public bool hasExploded = false;
    public Camera _camera;

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
    protected abstract void HandleExplosion();

    
}
