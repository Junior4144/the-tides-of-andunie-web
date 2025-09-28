using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public GameObject[] enemies;
    void Start()
    {
        // Optionally, find all enemies at the start
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
    }
    public void DestroyAllEnemies()
    {
        foreach (GameObject enemy in enemies)
        {
            if (enemy != null)
            {
                Destroy(enemy);
            }
        }
    }
}