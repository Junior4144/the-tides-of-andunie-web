using UnityEngine;

public class CannonBallStraight : CannonBall
{
    public float force;

    protected override void Shoot() =>
        rb.linearVelocity = Vector2.left * force;
}
