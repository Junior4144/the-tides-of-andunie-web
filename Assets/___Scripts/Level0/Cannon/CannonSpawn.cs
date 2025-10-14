using System.Collections;
using UnityEngine;

public class CannonSpawn : MonoBehaviour
{
    public GameObject cannon;
    private float timer;

    void Update()
    {
        
        timer += Time.deltaTime;

        if (timer >= 1)
        {
            SpawnCannonBall();
            timer = 0f;
        }
    }

    void SpawnCannonBall()
    {
        Instantiate(cannon, transform.position, Quaternion.identity);
    }


}