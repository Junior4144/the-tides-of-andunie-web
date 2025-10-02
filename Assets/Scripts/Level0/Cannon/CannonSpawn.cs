using UnityEngine;

public class CannonSpawn : MonoBehaviour
{

    public GameObject cannon;
    

    private float timer;

    void Update()
    {
        timer += Time.deltaTime;

        if(timer > 1)
        {
            timer = 0;
            spawnCannonBall();
        }
    }

    void spawnCannonBall()
    {
        Instantiate(cannon, transform.position, Quaternion.identity);
    }
}
