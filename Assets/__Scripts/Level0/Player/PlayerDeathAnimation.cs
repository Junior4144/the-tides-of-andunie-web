using UnityEngine;

public class PlayerDeathAnimation : MonoBehaviour
{
    public GameObject deathAnimation;
    
    public void DeathAnimation(){
        Instantiate(deathAnimation, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
