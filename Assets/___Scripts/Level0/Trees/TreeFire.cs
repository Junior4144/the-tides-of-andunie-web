using System.Collections.Generic;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;

public abstract class TreeFire : MonoBehaviour
{
    public GameObject fireSound;

    [SerializeField]
    private GameObject[] _fire_position_list;
    [SerializeField]
    private GameObject fireSprite_1;

    private bool _isLevelSelectorScene;

    private void OnEnable()
    {
        InSceneActivationManager.OnSceneActivated += StartLevelSelectorFire;
    }
    private void OnDisable()
    {
        InSceneActivationManager.OnSceneActivated -= StartLevelSelectorFire;
    }

    private void Awake()
    {
        CacheFirePoints();

        _isLevelSelectorScene = gameObject.scene.name == "LevelSelector";

        if (!_isLevelSelectorScene)
        {
            SpawnFire();
            SpawnFireSound();
        }
            
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
        int fireCount = _fire_position_list.Length;

        for (int i = 0; i < fireCount; i++)
        {
            Instantiate(fireSprite_1, _fire_position_list[i].transform.position, Quaternion.identity);
            if (_isLevelSelectorScene) break;
        }
    }
    private void SpawnFireSound()
    {
        if (fireSound != null)
            Instantiate(fireSound, transform.position, Quaternion.identity, transform);
    }

    private void StartLevelSelectorFire()
    {
        if (_isLevelSelectorScene)
            SpawnFire();
    }
}

