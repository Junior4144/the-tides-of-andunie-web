using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform player;
    public float minSpeed = 3f;
    public float maxSpeed = 5f;
    public float accelerationTime = 3f;
    private float currentSpeed = 0f;

    public Vector3 direction;
    public float edgeDistance = 10f;

    void Update()
    {
        float targetSpeed = player != null ? CalculateTargetSpeed() : minSpeed;
        MoveCamera(targetSpeed);
    }

    private float CalculateTargetSpeed() => Mathf.Lerp(minSpeed, maxSpeed, CalculateDistanceToCameraEdge());

    private float CalculateDistanceToCameraEdge()
    {
        float distanceInDirection = Vector3.Dot(player.position - transform.position, direction.normalized);
        return Mathf.Clamp01(distanceInDirection / edgeDistance);
    }

    private void MoveCamera(float targetSpeed)
    {
        currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, Time.deltaTime / accelerationTime);
        transform.position += direction.normalized * currentSpeed * Time.deltaTime;
    }
}