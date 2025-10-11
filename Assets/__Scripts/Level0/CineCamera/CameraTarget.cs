using UnityEngine;

public class CameraTarget : MonoBehaviour
{
    public static CameraTarget Instance { get; private set; }

    [SerializeField] private float scrollSpeed;
    [SerializeField] private float yInfluence;

    private Transform player;

    private Rigidbody2D playerRb;

    private float currentX;

    void Start()
    {
        player = PlayerManager.Instance.gameObject.transform;
        playerRb = player.GetComponent<Rigidbody2D>();
        currentX = transform.position.x;
    }
        

    void Update()
    {
        if (player == null || playerRb == null) return;

        float adjustedSpeed = CalculateXAxisMovement();

        currentX += -adjustedSpeed * Time.deltaTime;

        HandleYAxisMovement();
    }
    private float CalculateXAxisMovement()
    {
        float adjustedSpeed = scrollSpeed - Mathf.Abs(playerRb.linearVelocity.y) * yInfluence;
        return Mathf.Max(0f, adjustedSpeed);
    }
    private void HandleYAxisMovement() =>
        transform.position = new Vector3(currentX, player.position.y, transform.position.z);
}