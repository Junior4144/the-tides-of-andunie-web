using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

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
    public string VillageId => villageId;

    private SpriteRenderer spriteRenderer;

    [Header("Building Data (Shared ScriptableObject)")]
    [SerializeField] private VillageBuildingData _buildingData;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        List<GameObject> list = new List<GameObject>();

        for (int i = 0; i < transform.childCount; i++)
        {
            var child = transform.GetChild(i).gameObject;
            if (child.name == "VillagerSpawner" || child.name == "Particle")
                continue;

            list.Add(child);
        }

        firePositions = list.ToArray();
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



        // Fire logic
        if (bigBuilding) HandleFireBigBuilding();
        else if (smallBuilding) HandleFireSmallBuilding();
        else HandleFireUsingData(); // NEW: use ScriptableObject fallback

        // Audio logic
        if (fireSoundPrefab != null)
            SpawnFireSound();
        else
            SpawnFireSoundUsingData(); // NEW fallback

        // Sprite replace logic
        ReplaceSprite();
    }
    private void HandleFireBigBuilding()
    {
        SpawnFire(0.5f);
    }

    private void HandleFireSmallBuilding()
    {
        SpawnFire(0.25f);
    }
    private void SpawnFire(float scale)
    {
        if (fireSprites != null && fireSprites.Length > 0 && fireSprites[0] != null)
        {
            foreach (var position in firePositions)
            {
                GameObject fire = Instantiate(fireSprites[0], position.transform.position, Quaternion.identity);
                fire.transform.localScale = Vector3.one * scale;
            }
        }
        else
        {
            // Use data-based fire if no local prefab assigned
            SpawnFireUsingData(scale);
        }
    }

    private void SpawnFireSound()
    {
        //if (fireSoundPrefab != null)
            //Instantiate(fireSoundPrefab, transform.position, Quaternion.identity, transform);
    }
    private void ReplaceSprite()
    {
        if (spriteRenderer != null && destroyedSprite != null)
        {
            spriteRenderer.sprite = destroyedSprite;
            return;
        }

        ReplaceSpriteUsingData();
    }

    private void SpawnFireUsingData(float scale)
    {
        if (_buildingData == null || _buildingData.fireSpritePrefab == null)
        {
            Debug.Log($"{name}: No fire sprite found in scriptable object!");
            return;
        }

        foreach (var position in firePositions)
        {
            GameObject fire = Instantiate(_buildingData.fireSpritePrefab, position.transform.position, Quaternion.identity);
            fire.transform.localScale = Vector3.one * scale;
        }
    }

    private void HandleFireUsingData()
    {
        if (_buildingData == null || _buildingData.fireSpritePrefab == null)
        {
            Debug.Log($"{name}: No fire sprite assigned for data-based fallback.");
            return;
        }

        foreach (var position in firePositions)
        {
            GameObject fire = Instantiate(_buildingData.fireSpritePrefab, position.transform.position, Quaternion.identity);
            fire.transform.localScale = Vector3.one * 0.3f;
        }
    }

    private void SpawnFireSoundUsingData()
    {
        if (_buildingData == null || _buildingData.fireSoundPrefab == null)
            return;

        //Instantiate(_buildingData.fireSoundPrefab, transform.position, Quaternion.identity, transform);
    }

    private void ReplaceSpriteUsingData()
    {
        if (_buildingData == null)
            return;

        Sprite destroyed = _buildingData.GetDestroyedSprite(gameObject.name);
        if (destroyed != null)
            spriteRenderer.sprite = destroyed;
    }
}
