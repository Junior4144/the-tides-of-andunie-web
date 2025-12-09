using UnityEngine;

public class CameraTarget : MonoBehaviour
{
    private Transform playerTransform;

    [Header("Horizontal Auto-Scroll Settings")]
    public float baseScrollSpeed = 5f;
    public float slowdownRange = 5f;
    public float minScrollSpeed = 1f;

    [Header("Vertical Follow Settings")]
    public float yFollowSpeed = 3f;
    public bool smoothFollow = true;

    void Start()
    {
        playerTransform = PlayerManager.Instance.GetPlayerTransform();
    }

    void Update()
    {
        if (playerTransform == null) return;

        Vector3 targetPosition = transform.position;

        float distanceBehind = playerTransform.position.x - transform.position.x;
        float speed = baseScrollSpeed;

        if (distanceBehind > 0)
        {
            float t = Mathf.Clamp01(distanceBehind / slowdownRange);
            speed = Mathf.Lerp(baseScrollSpeed, minScrollSpeed, t);
        }

        targetPosition.x -= speed * Time.deltaTime;


        if (smoothFollow)
        {
            targetPosition.y = Mathf.Lerp(transform.position.y, playerTransform.position.y, Time.deltaTime * yFollowSpeed);
        }
        else
        {
            targetPosition.y = playerTransform.position.y;
        }

        transform.position = new Vector3(targetPosition.x, targetPosition.y, transform.position.z);
    }
}