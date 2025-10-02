using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float targetSpeed = 3f;   
    public float accelerationTime = 3f; 
    private float currentSpeed = 0f;

    void Update()
    {
        currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, Time.deltaTime / accelerationTime);
        transform.position += Vector3.left * currentSpeed * Time.deltaTime;
    }
}
