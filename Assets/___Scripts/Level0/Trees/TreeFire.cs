using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public abstract class TreeFire : MonoBehaviour
{
    public GameObject fireSound;

    [SerializeField]
    protected GameObject[] _fire_position_list;

    [SerializeField]
    protected GameObject fireSprite_1;

    [SerializeField] protected GameObject firePositions;

    private void Awake()
    {

        int fireCount = transform.childCount;
        _fire_position_list = new GameObject[fireCount];

        for (int i = 0; i < fireCount; i++)
        {
            _fire_position_list[i] = transform.GetChild(i).gameObject;
        }

        SpawnNewFire();
    }
    protected abstract void SpawnNewFire();
}
