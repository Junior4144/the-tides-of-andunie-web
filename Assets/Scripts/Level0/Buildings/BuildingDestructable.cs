using Unity.Cinemachine;
using UnityEngine;

public abstract class BuildingDestructable : MonoBehaviour
{
    public GameObject explosion;
    public GameObject fire;
    public GameObject fireSound;
    public bool hasExploded = false;
    private Camera _camera;

    [SerializeField]
    private Sprite _spriteRenderer;
    [SerializeField]
    private GameObject _fire_1;
    [SerializeField]
    private GameObject _fire_2;
    [SerializeField]
    private GameObject _fire_3;

    [SerializeField]
    private GameObject fireSprite;

    private SpriteRenderer currentSprite;
    private CinemachineImpulseSource _impulseSource;

    private void Start() =>
        _impulseSource = GetComponent<CinemachineImpulseSource>();

    private void LateUpdate() => _camera = Camera.main;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (
            !collision.gameObject.CompareTag("CannonBall") ||
            hasExploded ||
            !CheckCameraBoundaries(GetScreenPosition())
           ) return;

        HandleExplosion();
        HandleBuildingCameraShake();
    }

    public bool CheckCameraBoundaries(Vector2 screenPosition, float padding = 500f) //like an inch lol
    {
        return screenPosition.x > 0 - padding;
    }
    public void SpawnExplosion() => Instantiate(explosion, transform.position, Quaternion.identity);

    public Vector2 GetScreenPosition() => _camera.WorldToScreenPoint(transform.position);

    public void HandleBuildingCameraShake()
    {
        CameraShakeManager.instance.CameraShake(_impulseSource);
    }

    protected void SpawnFire(Vector2[] offsets)
    {
        foreach (var offset in offsets)
            Instantiate(fire, transform.position + (Vector3)offset, Quaternion.identity);
    }
    protected void SpawnNewFire()
    {
        Instantiate(fireSprite, _fire_1.transform.position, Quaternion.identity);
        Instantiate(fireSprite, _fire_2.transform.position, Quaternion.identity);
        Instantiate(fireSprite, _fire_3.transform.position, Quaternion.identity);
    }

    protected void ReplaceSprite()
    {
        currentSprite = GetComponent<SpriteRenderer>();
        currentSprite.sprite = _spriteRenderer;
    }

    protected void SpawnFireSound()
    {
        if (fireSound != null)
            Instantiate(fireSound, transform.position, Quaternion.identity, transform);
    }

    protected void PlayExplosionSound(AudioClip explosionSound)
    {
        if (explosionSound != null)
            AudioSource.PlayClipAtPoint(explosionSound, transform.position, 1.0f);
        else
            Debug.LogError("ExplosionSound is Null. Playing no Sound");
    }


    protected abstract void HandleExplosion();
}
