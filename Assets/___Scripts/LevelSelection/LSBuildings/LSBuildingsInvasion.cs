using System.Collections;
using System.Collections.Generic;
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
    public string VillageId => villageId;

    private SpriteRenderer spriteRenderer;

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

    private void OnEnable()
    {
        LSManager.UpdateVillageInvasionStatus += HandleInvasion;
    }
    private void OnDisable()
    {
        LSManager.UpdateVillageInvasionStatus -= HandleInvasion;
    }

    private void HandleInvasion()
    {
        if (!gameObject.scene.name.Contains("LevelSelector"))
            return;

        if (string.IsNullOrEmpty(villageId))
            return;

        if (LSManager.Instance.GetVillageState(villageId) != VillageState.Invaded)
            return;



        if (bigBuilding) HandleFireBigBuilding();
        else if (smallBuilding) HandleFireSmallBuilding();

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
        if (fireSprites == null || fireSprites.Length == 0 || fireSprites[0] == null)
        {
            Debug.Log($"{name}: fireSprites not assigned or empty!");
            return;
        }

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
