using Unity.Cinemachine;
using UnityEngine;

public abstract class BuildingDestructable : MonoBehaviour
{
    [Header("Explosion Settings")]
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private GameObject explosionSoundPrefab;
    [SerializeField] private float explosionRadius = 15f;

    [Header("Fire Settings")]
    [SerializeField] private bool hasFire;
    [SerializeField] private GameObject fireSoundPrefab;
    [SerializeField] private GameObject[] firePositions;
    [SerializeField] private GameObject[] fireSprites;

    [Header("Sprite Settings")]
    [SerializeField] private Sprite destroyedSprite;

    [Header("Villager Settings")]
    [SerializeField] private GameObject villagerSpawner;
    [SerializeField] private GameObject villagerPrefab;

    public bool hasExploded;
    private GameObject player;
    private Camera mainCamera;
    private CinemachineImpulseSource impulseSource;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        impulseSource = GetComponent<CinemachineImpulseSource>();

        int fireCount = Mathf.Min(transform.childCount, 5);
        firePositions = new GameObject[fireCount];
        for (int i = 0; i < fireCount; i++)
            firePositions[i] = transform.GetChild(i).gameObject;


        if (hasFire)
        {
            SpawnFire();
            SpawnFireSound();
        }

    }

    private void Start()
    {
        player = PlayerManager.Instance.gameObject;
        mainCamera = Camera.main;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {        
        if (!collision.gameObject.CompareTag("CannonBall")) return;
        
        if (hasExploded) return;
        
        if (collision.gameObject.CompareTag("Enemy")) return;

        if (mainCamera == null) mainCamera = Camera.main;

        if (!IsVisibleOnCameraLeft()) return;
        
        HandleExplosion();

        HandleCameraShake();
    }

    private void HandleExplosion()
    {
        if (explosionPrefab != null)
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);;

        if (explosionSoundPrefab != null)
            Instantiate(explosionSoundPrefab, transform.position, Quaternion.identity, transform);

        ReplaceSprite();

        if (!hasFire)
        {
            SpawnFire();
            SpawnFireSound();
        }

        hasExploded = true;
    }

    private void ReplaceSprite()
    {
        if (spriteRenderer != null && destroyedSprite != null)
            spriteRenderer.sprite = destroyedSprite;
    }

    private void SpawnFire()
    {
        foreach (var position in firePositions)
        {
            Instantiate(fireSprites[0], position.transform.position, Quaternion.identity);
        }
    }

    private void SpawnFireSound()
    {
        if (fireSoundPrefab != null)
            Instantiate(fireSoundPrefab, transform.position, Quaternion.identity, transform);
    }

    private void SpawnVillager()
    {
        if (villagerSpawner != null && villagerPrefab != null)
            Instantiate(villagerPrefab, villagerSpawner.transform.position, Quaternion.identity);
    }

    private void HandleCameraShake()
    {
        if (player == null || impulseSource == null) return;

        float distance = Vector3.Distance(player.transform.position, transform.position);
        if (distance < explosionRadius)
            CameraShakeManager.instance?.CameraShake(impulseSource);
    }

    private bool IsVisibleOnCameraLeft(float padding = 500f)
    {
        if (mainCamera == null) return false;

        Vector2 screenPos = mainCamera.WorldToScreenPoint(transform.position);
        return screenPos.x > -padding;
    }
}
