using UnityEngine;

public class CannonStopper : MonoBehaviour
{

    void Update()
    {
        
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("CannonBall"))
        {
            // Do normal stop behavior
        }
        else
        {
            // Ignore collision
            Physics2D.IgnoreCollision(collision.collider, GetComponent<Collider2D>());
        }
    }
}
