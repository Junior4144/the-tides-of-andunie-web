using UnityEngine;

public class CannonShoot : MonoBehaviour
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

        if(timer > 2)
        {
            timer = 0;
            shoot();
        }
    }

    void shoot()
    {
        Instantiate(cannon, cannonPosition.position, Quaternion.identity);
    }
}
