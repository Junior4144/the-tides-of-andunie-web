using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VillageInvasionBuildings : MonoBehaviour
{
    [Header("Fire Settings")]
    [SerializeField] private GameObject fireSoundPrefab;
    [SerializeField] private GameObject[] firePositions;
    [SerializeField] private GameObject[] fireSprites;

    [Header("Sprite Settings")]
    [SerializeField] private Sprite destroyedSprite;

    [Header("Village Settings")]
    [SerializeField] private string villageId;

    [SerializeField] private VillageBuildingData _buildingData;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        int fireCount = Mathf.Min(transform.childCount, 5);
        firePositions = new GameObject[fireCount];
        for (int i = 0; i < fireCount; i++)
            firePositions[i] = transform.GetChild(i).gameObject;
    }

    private void OnEnable() => SceneManager.activeSceneChanged += HandleCheck;

    private void OnDisable() => SceneManager.activeSceneChanged -= HandleCheck;

    private void HandleCheck(Scene oldScene, Scene newScene)
    {
        StartCoroutine(CheckAfterLoading(newScene));
    }

    private IEnumerator CheckAfterLoading(Scene newScene)
    {
        yield return null;

        if (newScene == gameObject.scene)
            ApplyCurrentState();
    }

    private void ApplyCurrentState()
    {
        if (LSManager.Instance.HasInvasionStarted)
            HandleInvasion();
    }
    private void HandleInvasion()
    {
        if (string.IsNullOrEmpty(villageId))
        {
            if (VillageIDManager.Instance)
            {
                villageId = VillageIDManager.Instance.villageId;
            }
        }
            

       

        if (LSManager.Instance.GetVillageState(villageId) != VillageState.Invaded)
            return;

        ReplaceSprite();

        HandleFireBuilding();
    }
    private void HandleFireBuilding()
    {
        if(fireSprites.Length > 0)
        {
            SpawnFire();
        }
        else
        {
            SpawnFireUsingData();
        }

        if (fireSoundPrefab != null)
        {
            SpawnFireSound();
        }
        else
        {
            if (_buildingData == null) return;
            SpawnFireSoundUsingData();
        }
        
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
    private void ReplaceSprite()
    {
        if (spriteRenderer != null && destroyedSprite != null)
            spriteRenderer.sprite = destroyedSprite;
        else
        {
            ReplaceSpriteUsingData();
        }
    }

    private void ReplaceSpriteUsingData()
    {
        Sprite destroyedSprite = _buildingData.GetDestroyedSprite(gameObject.name);
        if (destroyedSprite != null)
            spriteRenderer.sprite = destroyedSprite;
    }

    private void SpawnFireSoundUsingData()
    {
        Instantiate(_buildingData.fireSoundPrefab, transform.position, Quaternion.identity, transform);
    }

    private void SpawnFireUsingData()
    {
        foreach (var position in firePositions)
        {
            Instantiate(_buildingData.fireSpritePrefab, position.transform.position, Quaternion.identity);
        }
    }
}
