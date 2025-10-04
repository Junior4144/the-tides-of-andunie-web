using UnityEngine;

public class CameraTarget : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Rigidbody2D playerRb;
    [SerializeField] private float scrollSpeed;
    [SerializeField] private float yInfluence;

    private float currentX;

    void Start()
    {
        currentX = transform.position.x;
    }

    void Update()
    {
        float adjustedSpeed = CalculateXAxisMovement();

        currentX += -adjustedSpeed * Time.deltaTime;

        HandleYAxisMovement();
    }
    private float CalculateXAxisMovement()
    {
        float adjustedSpeed = scrollSpeed - Mathf.Abs(playerRb.linearVelocity.y) * yInfluence;
        return Mathf.Max(0f, adjustedSpeed);
    }
    private void HandleYAxisMovement()
    {
        transform.position = new Vector3(currentX, player.position.y, transform.position.z);
    }
}