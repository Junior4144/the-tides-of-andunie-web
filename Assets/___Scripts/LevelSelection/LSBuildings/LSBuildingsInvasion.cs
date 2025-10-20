using System.Collections;
using UnityEngine;

public class LSBuildingsInvasion : MonoBehaviour
{

    public bool bigBuilding = false;
    public bool smallBuilding = false;

    [Header("Fire Settings")]
    [SerializeField] private GameObject fireSoundPrefab;
    [SerializeField] private GameObject[] firePositions;
    [SerializeField] private GameObject[] fireSprites;

    [Header("Sprite Settings")]
    [SerializeField] private Sprite destroyedSprite;

    [Header("Village Settings")]
    [SerializeField] private string villageId;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        int fireCount = Mathf.Min(transform.childCount, 5);
        firePositions = new GameObject[fireCount];
        for (int i = 0; i < fireCount; i++)
            firePositions[i] = transform.GetChild(i).gameObject;
    }

    private void OnEnable()
    {
        LSManager.OnGlobalInvasionStarted += HandleInvasion;

    }
    private void OnDisable()
    {
        LSManager.OnGlobalInvasionStarted -= HandleInvasion;
    }
    private void Start()
    {
        StartCoroutine(ApplyCurrentState());
    }
    private IEnumerator ApplyCurrentState()
    {
        yield return null;
        if (LSManager.Instance.HasInvasionStarted)
            HandleInvasion();
    }
    private void HandleInvasion()
    {
        if (!gameObject.scene.name.Contains("LevelSelector"))
            return;

        if (string.IsNullOrEmpty(villageId))
            return;

        if (LSManager.Instance.GetVillageState(villageId) != VillageState.Invaded)
            return;

        ReplaceSprite();

        if (bigBuilding) HandleFireBigBuilding();
        else if (smallBuilding) HandleFireSmallBuilding();
    }
    private void HandleFireBigBuilding()
    {
        SpawnFire(0.5f);
        SpawnFireSound();
    }

    private void HandleFireSmallBuilding()
    {
        SpawnFire(0.25f);
    }
    private void SpawnFire(float scale)
    {
        foreach (var position in firePositions)
        {
            GameObject fire = Instantiate(fireSprites[0], position.transform.position, Quaternion.identity);
            fire.transform.localScale = Vector3.one * scale;
        }
    }

    private void SpawnFireSound()
    {
        if (fireSoundPrefab != null)
            Instantiate(fireSoundPrefab, transform.position, Quaternion.identity, transform);
    }
    private void ReplaceSprite()
    {
        if (spriteRenderer != null && destroyedSprite != null)
            spriteRenderer.sprite = destroyedSprite;
    }
}
