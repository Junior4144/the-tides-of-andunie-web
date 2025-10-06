using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

public abstract class BuildingDestructable : MonoBehaviour
{
    
    public GameObject explosion;
    public GameObject fire;
    public GameObject fireSound;
    public bool hasExploded = false;
    private bool hasSpawnedVillager = false;
    private Camera _camera;

    private GameObject player;

    private float explosionRadius = 15f;

    [SerializeField]
    private Sprite _spriteRenderer;
    [SerializeField]
    private AudioClip _explosionSound;
    [SerializeField]
    private AudioClip _fireSound;

    [SerializeField]
    private GameObject _fire_position_1;
    [SerializeField]
    private GameObject _fire_position_2;
    [SerializeField]
    private GameObject _fire_position_3;
    [SerializeField]
    private GameObject _fire_position_4;
    [SerializeField]
    private GameObject _fire_position_5;


    [SerializeField]
    private GameObject fireSprite_1;
    [SerializeField]
    private GameObject fireSprite_2;
    [SerializeField]
    private GameObject fireSprite_3;
    [SerializeField]
    private GameObject fireSprite_4;
    [SerializeField]
    private GameObject fireSprite_5;

    [SerializeField] public GameObject _villagerSpawner;
    [SerializeField] public GameObject _villager;

    private SpriteRenderer currentSprite;
    private CinemachineImpulseSource _impulseSource;
    

    private void Start() =>
        _impulseSource = GetComponent<CinemachineImpulseSource>();

    private void LateUpdate() => _camera = Camera.main;

    private void Awake() =>
        player = GameObject.FindWithTag("Player");

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (
            !collision.gameObject.CompareTag("CannonBall") ||
            hasExploded ||
            !CheckCameraLeftBoundary(GetScreenPosition())
        ) return;

        HandleExplosion();

        //if(!hasSpawnedVillager) HandleVillager();

        if(player == null) return;
        float distance = Vector3.Distance(player.transform.position, transform.position);

        if (player != null && distance < explosionRadius)
        {
            HandleBuildingCameraShake();
        }
            

    }

    public bool CheckCameraLeftBoundary(Vector2 screenPosition, float padding = 500f) //like an inch 
    {
        return screenPosition.x > 0 - padding;
    }
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

    protected void SpawnNewFire()
    {
        Instantiate(fireSprite_1, _fire_position_1.transform.position, Quaternion.identity);
        Instantiate(fireSprite_2, _fire_position_2.transform.position, Quaternion.identity);
        Instantiate(fireSprite_3, _fire_position_3.transform.position, Quaternion.identity);
        Instantiate(fireSprite_4, _fire_position_4.transform.position, Quaternion.identity);
        Instantiate(fireSprite_5, _fire_position_5.transform.position, Quaternion.identity);

    }
    private void HandleExplosion()
    {
        SpawnExplosion();
        //PlayExplosionSound(_explosionSound);
        SoundFxManager.instance.PlayerSoundFxClip(_explosionSound, transform, 1f);

        ReplaceSprite();

        SpawnNewFire();
        SpawnFireSound();
        //SoundFxManager.instance.PlayerSoundFxClip(_fireSound, transform, 1f);

        hasExploded = true;
    }
    private void HandleVillager()
    {
        Instantiate(_villager, _villagerSpawner.transform.position, Quaternion.identity);
        hasSpawnedVillager = true;
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

}
