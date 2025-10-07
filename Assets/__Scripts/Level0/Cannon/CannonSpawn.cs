using System.Collections;
using UnityEngine;

public class CannonSpawn : MonoBehaviour
{
    public GameObject cannon;
    private float timer;
    private float timerForSound;

    public GameObject CannonSingleSound;

    public GameObject CannonTrippleShot;

    // Range for random spawn intervals
    float minSpawnTime = 1f;
    private float maxSpawnTime = 2f;

    private float nextSpawnTime;

    void Start()
    {
        nextSpawnTime = Random.Range(minSpawnTime, maxSpawnTime);
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= 1)
        {
            SpawnCannonBall();
            timer = 0f;
            if (!CannonTrippleShot)
            {
                Instantiate(CannonSingleSound, transform.position, Quaternion.identity);
                return;

            }

            Instantiate(CannonTrippleShot, transform.position, Quaternion.identity);
        }
        if(timerForSound > 2)
        {
            //StartCoroutine(SpawnCannonShotSound());
            timerForSound = 0f;
        }
    }

    void SpawnCannonBall()
    {
        Instantiate(cannon, transform.position, Quaternion.identity);
    }


}
//every 2 seconds -> 3 cannonshot sounds