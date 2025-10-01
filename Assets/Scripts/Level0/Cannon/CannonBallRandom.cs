using UnityEngine;
using UnityEngine.Events;

public class CannonBallRandom : CannonBall
{
    public float force;

    [Header("Random Target Settings")]
    public float minY = -10f;
    public float maxY = 10f;

    protected override void Shoot()
    {
        rb.linearVelocity = (GetRandomPosition() - transform.position).normalized * force;
    }
        
    public Vector3 GetRandomPosition() =>
        new Vector3(0f, Random.Range(minY, maxY), 0f);
}
