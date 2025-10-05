using UnityEngine;

public class CannonBallPlayer : CannonBall
{
    private GameObject player;
    public float force;

    protected override void Shoot()
    {
        if (player == null)
        {
            Destroy(gameObject);
            return;
        }
        player = GameObject.FindGameObjectWithTag("Player");
        rb.linearVelocity = (player.transform.position - transform.position).normalized * force;
    }
}
