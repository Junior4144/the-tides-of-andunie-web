using UnityEngine;

public abstract class BuildingDestructable : MonoBehaviour
{
    public GameObject explosion;
    public GameObject fire;
    public bool hasExploded = false;
    public Camera _camera;

    private void LateUpdate() =>
        _camera = Camera.main;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("CannonBall")) return;

        HandleExplosion();

    }
    public bool CheckCameraBoundaries(Vector2 screenPosition)
    {
        return (screenPosition.x >= 0 &&
            screenPosition.x <= _camera.pixelWidth &&
            screenPosition.y >= 0 &&
            screenPosition.y <= _camera.pixelHeight)
            ? true : false;
    }
    public void SpawnExplosion() =>
        Instantiate(explosion, transform.position, Quaternion.identity);

    public Vector2 GetScreenPosition() =>
     _camera.WorldToScreenPoint(transform.position);

    protected abstract void HandleExplosion();

    
}
