using System.Collections;
using UnityEngine;

public class CannonSpawn : MonoBehaviour
{
    public GameObject cannon;
    private float timer;
    private float timerForSound;

    [SerializeField]
    private AudioClip cannonShotSound;

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
        //timerForSound += Time.deltaTime;

        if (timer >= 1)
        {
            SpawnCannonBall();
            timer = 0f;
            SoundFxManager.instance.PlayerSoundFxClip(cannonShotSound, transform, 1f);

            //nextSpawnTime = Random.Range(minSpawnTime, maxSpawnTime);
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

    private IEnumerator SpawnCannonShotSound()
    {
        SoundFxManager.instance.PlayerSoundFxClip(cannonShotSound, transform, 1f);
        yield return new WaitForSeconds(0.3f);
        SoundFxManager.instance.PlayerSoundFxClip(cannonShotSound, transform, 1f);
        yield return new WaitForSeconds(0.3f);
        SoundFxManager.instance.PlayerSoundFxClip(cannonShotSound, transform, 1f);

    }
}
//every 2 seconds -> 3 cannonshot sounds