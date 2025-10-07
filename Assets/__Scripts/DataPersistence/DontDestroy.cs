using System;
using UnityEngine;

public class DontDestroy : MonoBehaviour
{
    private static GameObject[] persistentObjects = new GameObject[3];
    public int objectIndex;
    void Awake()
    {
        if (!persistentObjects[objectIndex])
        {
            persistentObjects[objectIndex] = gameObject;
            DontDestroyOnLoad(gameObject);
        }
        else if (persistentObjects[objectIndex] != gameObject)
        {
            Destroy(gameObject);
        }
    }
}
