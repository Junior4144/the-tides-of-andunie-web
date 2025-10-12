using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class EnemyData
{
    public GameObject prefab;
    public int count;
}


[System.Serializable]
public class WaveConfig
{
    [Tooltip("Enemies that spawn during this wave")]
    public List<EnemyData> enemies;

    [Tooltip("Duration before the next wave begins (seconds)")]
    public float countdown = 30f;

    [Tooltip("The time in seconds between each enemy spawn.")]
    public float spawnInterval = 0.5f;
}