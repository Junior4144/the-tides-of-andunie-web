using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

public abstract class BuildingDestructable : MonoBehaviour
{
    public GameObject explosion;
    public GameObject fire;
    public GameObject fireSound;
    public bool hasExploded = false;
    public bool hasFire = false;

    [SerializeField]
    private Sprite _spriteRenderer;

    [SerializeField]
    private GameObject _explosionSoundPrehab;

    [SerializeField]
    public GameObject[] _fire_Positions;
    [SerializeField]
    public GameObject[] _fire_Sprites;

    [SerializeField] public GameObject _villagerSpawner;
    [SerializeField] public GameObject _villager;

    private Camera _camera;
    private GameObject player;
    private float explosionRadius = 15f;
    private SpriteRenderer currentSprite;
    private CinemachineImpulseSource _impulseSource;


    private void Start() =>
        _impulseSource = GetComponent<CinemachineImpulseSource>();

    private void LateUpdate() => _camera = Camera.main;

    private void Awake()
    {
        player = GameObject.FindWithTag("Player");
        if (hasFire)
        {
            SpawnNewFire();
            SpawnFireSound();
        }
    }  

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!_camera)
        { 
            HandleExplosion();
            return;
        }

        if (   
            !collision.gameObject.CompareTag("CannonBall") ||
            hasExploded ||
            !CheckCameraLeftBoundary(GetScreenPosition()) ||
            collision.gameObject.CompareTag("Enemy")
        ) return;

        HandleExplosion();

        if (player == null) return;

        float distance = Vector3.Distance(player.transform.position, transform.position);

        if (player != null && distance < explosionRadius)
            HandleBuildingCameraShake();
    }

    public bool CheckCameraLeftBoundary(Vector2 screenPosition, float padding = 500f) => //like an inch
        screenPosition.x > 0 - padding;

    public bool CheckCameraAllBoundary(Vector2 screenPosition, float padding = 0f)
    {
        return screenPosition.x > -padding &&
               screenPosition.x < Screen.width + padding &&
               screenPosition.y > -padding &&
               screenPosition.y < Screen.height + padding;
    }

    public void SpawnExplosion() => Instantiate(explosion, transform.position, Quaternion.identity);

    public Vector2 GetScreenPosition() => _camera.WorldToScreenPoint(transform.position);

    public void HandleBuildingCameraShake() =>
        CameraShakeManager.instance.CameraShake(_impulseSource);

    private void HandleExplosion()
    {
        SpawnExplosion();

        if (explosion != null)
            Instantiate(_explosionSoundPrehab, transform.position, Quaternion.identity, transform);

        ReplaceSprite();

        if (hasFire) return;

        SpawnNewFire();
        SpawnFireSound();

        hasExploded = true;
    }

    protected void SpawnNewFire()
    {
        for(int i = 0; i < _fire_Positions.Length; i++)
            Instantiate(_fire_Sprites[0], _fire_Positions[i].transform.position, Quaternion.identity);
    }

    private void HandleVillager()
    {
        Instantiate(_villager, _villagerSpawner.transform.position, Quaternion.identity);
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
}
