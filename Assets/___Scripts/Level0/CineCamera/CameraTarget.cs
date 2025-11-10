using UnityEngine;

public class CameraTarget : MonoBehaviour
{

    [SerializeField] private float baseScrollSpeed = 5f;
    [SerializeField] private float yInfluence = 0.5f;
    [SerializeField] private float catchupMultiplier = 2f;
    [SerializeField] private float maxDistanceBeforeBoost = 3f;

    private Transform player;
    private Rigidbody2D playerRb;
    private float currentX;

    void Start()
    {
        player = PlayerManager.Instance.GetPlayerTransform();
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
        player = PlayerManager.Instance.GetPlayerTransform();
        // Base scrolling speed (decreased by vertical movement)
        float adjustedSpeed = baseScrollSpeed - Mathf.Abs(playerRb.linearVelocity.y) * yInfluence;
        adjustedSpeed = Mathf.Max(0f, adjustedSpeed);

        // Calculate distance between camera target and player
        float distance = player.position.x - transform.position.x;

        // If the player is close to overtaking (ahead), speed up scrolling
        if (distance > 0)
        {
            float boostFactor = Mathf.Clamp01(distance / maxDistanceBeforeBoost);
            adjustedSpeed += boostFactor * catchupMultiplier;
        }

        return adjustedSpeed;
    }

    private void HandleYAxisMovement()
    {
        transform.position = new Vector3(currentX, player.position.y, transform.position.z);
    }
}