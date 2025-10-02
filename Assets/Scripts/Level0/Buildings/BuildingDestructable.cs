using UnityEngine;

public abstract class BuildingDestructable : MonoBehaviour
{
    public GameObject explosion;
    public GameObject fire;
    public GameObject fireSound;
    public bool hasExploded = false;
    private Camera _camera;

    private void LateUpdate() => _camera = Camera.main;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (
            !collision.gameObject.CompareTag("CannonBall") ||
            hasExploded ||
            !CheckCameraBoundaries(GetScreenPosition())
        ) return;
 
        HandleExplosion();
    }

    public bool CheckCameraBoundaries(Vector2 screenPosition)
    {
        return (
            screenPosition.x >= 0 &&
            screenPosition.x <= _camera.pixelWidth &&
            screenPosition.y >= 0 &&
            screenPosition.y <= _camera.pixelHeight
        );
    }

    public void SpawnExplosion() => Instantiate(explosion, transform.position, Quaternion.identity);

    public Vector2 GetScreenPosition() => _camera.WorldToScreenPoint(transform.position);

    protected void SpawnFire(Vector2[] offsets)
    {
        foreach (var offset in offsets)
            Instantiate(fire, transform.position + (Vector3)offset, Quaternion.identity);
    }

    protected void SpawnFireSound()
    {
        if (fireSound != null)
        {
            Instantiate(fireSound, transform.position, Quaternion.identity, transform);
        }
    }

    protected void PlayExplosionSound(AudioClip explosionSound)
    {
        if (explosionSound != null)
        {
            AudioSource.PlayClipAtPoint(explosionSound, transform.position, 1.0f);
        }
        else
        {
            Debug.LogError("ExplosionSound is Null. Playing no Sound");
        }
    }

    protected abstract void HandleExplosion();
}
