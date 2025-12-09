using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class TreeFire : MonoBehaviour
{
    public GameObject fireSound;

    [SerializeField]
    private GameObject[] _fire_position_list;
    [SerializeField]
    private GameObject fireSprite_1;

    private bool _isLevelSelectorScene = false;

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
        {
            if(gameObject.scene.name == "LevelSelector") _isLevelSelectorScene = true;
            SpawnFire();
        }
            
    }

    private void Awake()
    {
        CacheFirePoints();    
    }

    private void CacheFirePoints()
    {
        int fireCount = transform.childCount;
        _fire_position_list = new GameObject[fireCount];

        for (int i = 0; i < fireCount; i++)
        {
            _fire_position_list[i] = transform.GetChild(i).gameObject;
        }
    }

    private void SpawnFire()
    {
        if (LSManager.Instance.GetVillageState("Village9") == VillageState.Liberated_Done)
            return;

        int fireCount = _fire_position_list.Length;

        for (int i = 0; i < fireCount; i++)
        {
            Instantiate(fireSprite_1, _fire_position_list[i].transform.position, Quaternion.identity);
            if (_isLevelSelectorScene) break;
        }
    }
    //private void SpawnFireSound()
    //{
    //    if (fireSound != null)
    //        Instantiate(fireSound, transform.position, Quaternion.identity, transform);
    //}

}

