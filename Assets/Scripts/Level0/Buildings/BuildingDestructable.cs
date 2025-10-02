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
        return (screenPosition.x >= -3f &&
            screenPosition.x <= _camera.pixelWidth + 3f &&
            screenPosition.y >= -3f &&
            screenPosition.y <= _camera.pixelHeight + 3f)
            ? true : false;
    }
    public void SpawnExplosion() =>
        Instantiate(explosion, transform.position, Quaternion.identity);

    public Vector2 GetScreenPosition() =>
     _camera.WorldToScreenPoint(transform.position);

    protected abstract void HandleExplosion();

    
}
