using UnityEngine;

public class CannonShotSound : MonoBehaviour
{

    private float timer;
    public GameObject CannonSingleSound;
    public GameObject CannonTrippleShot;

    void Update()
    {

        timer += Time.deltaTime;

        if (timer >= 1)
        {
            //for cutscene
            if (!CannonTrippleShot)
            {
                Instantiate(CannonSingleSound, transform.position, Quaternion.identity);
                return;
            }
            Instantiate(CannonTrippleShot, transform.position, Quaternion.identity);

            timer = 0f;
        }
    }




}
