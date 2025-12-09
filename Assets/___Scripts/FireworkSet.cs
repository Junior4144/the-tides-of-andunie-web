using UnityEngine;

[CreateAssetMenu(fileName = "FireworkSet", menuName = "Fireworks/Firework Set")]
public class FireworkSet : ScriptableObject
{
    public GameObject[] fireworkPrefabs;
    public AudioClip fireworkClip;

    [Header("Wave Settings")]
    public int[] spawnCounts;
    public float[] waveDelays;
    
    private void OnValidate()
    {
        if (spawnCounts.Length != waveDelays.Length)
        {
            Debug.LogWarning($"{name}: spawnCounts and waveDelays must have same length!");
        }
    }
}
