using UnityEngine;

public class CannonBallRandom : CannonBall
{
    public float force = 10f;

    [Header("Random Target Settings")]
    public float minY = -10f;
    public float maxY = 10f;
    public float minXOffset = -15f;
    public float maxXOffset = -5f;

    protected override void Shoot()
    {
        Vector3 target = GetRandomPosition();
        Vector3 direction = (target - transform.position).normalized;
        rb.linearVelocity = direction * force;
    }

    public Vector3 GetRandomPosition()
    {
        float randomX = transform.position.x + Random.Range(minXOffset, maxXOffset);
        float randomY = transform.position.y + Random.Range(minY, maxY);
        return new Vector3(randomX, randomY, transform.position.z);
    }
}
