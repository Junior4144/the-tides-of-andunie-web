using UnityEngine;

public class CannonSpawn : MonoBehaviour
{

    public GameObject cannon;
    public Transform cannonPosition;

    private float timer;
    void Start()
    {
        
    }
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
        Instantiate(cannon, cannonPosition.position, Quaternion.identity);
    }
}
