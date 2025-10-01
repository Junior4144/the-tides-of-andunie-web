using UnityEngine;

public class CannonBallPlayer : CannonBall
{
    private GameObject player;
    public float force;

    protected override void Shoot()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rb.linearVelocity = (player.transform.position - transform.position).normalized * force;
    }
}
