using System.Collections;
using UnityEngine;

public class VillageInvasionBuildings : MonoBehaviour
{
    [Header("Fire Settings")]
    [SerializeField] private GameObject fireSoundPrefab;
    [SerializeField] private GameObject[] firePositions;
    [SerializeField] private GameObject[] fireSprites;

    //[Header("Sprite Settings")]
    //[SerializeField] private Sprite destroyedSprite;

    [Header("Village Settings")]
    [SerializeField] private string villageId;

    //private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        //spriteRenderer = GetComponent<SpriteRenderer>();
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
        if (string.IsNullOrEmpty(villageId))
            return;

        if (LSManager.Instance.GetVillageState(villageId) != VillageState.Invaded)
            return;

        //ReplaceSprite();

        HandleFireBuilding();
    }
    private void HandleFireBuilding()
    {
        SpawnFire();
        SpawnFireSound();
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
    //private void ReplaceSprite()
    //{
    //    if (spriteRenderer != null && destroyedSprite != null)
    //        spriteRenderer.sprite = destroyedSprite;
    //}
}
