using UnityEngine;
using System.Collections.Generic;
using System.Linq;

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

    [Tooltip("The text that appears when the countdown starts.")]
    public string countDownText = "Enemies observed in the distance! Prepare to defend!!!";

    [Tooltip("The text that appears when the enemies start spawning.")]
    public string waveStartText = "Enemies are here! Attack!!!";
    public float totalDuration => countdown + spawnInterval * enemies.Sum(enemyData => enemyData.count);
}