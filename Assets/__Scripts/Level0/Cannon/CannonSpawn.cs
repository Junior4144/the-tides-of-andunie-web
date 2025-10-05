using UnityEngine;

public class CannonSpawn : MonoBehaviour
{
    public GameObject cannon;
    [SerializeField]
    private GameObject cannonShotSound;
    private float timer;

    void Update()
    {
        timer += Time.deltaTime;
        if(timer > 1)
        {
            timer = 0;
            SpawnCannonBall();
        }
    }

    void SpawnCannonBall()
    {
        Instantiate(cannonShotSound, transform.position, Quaternion.identity);
        Instantiate(cannon, transform.position, Quaternion.identity);
    }
}
