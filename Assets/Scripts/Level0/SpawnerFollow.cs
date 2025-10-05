using UnityEngine;

public class FollowCameraTarget : MonoBehaviour
{
    [SerializeField] private CameraTarget target;
    [SerializeField] private float followSpeed = 5f; // how fast it catches up

    private Vector3 initialOffset;

    void Start()
    {
        if (target != null)
            initialOffset = transform.position - target.transform.position;
    }

    void LateUpdate()
    {
        if (target == null) return;

        // Desired position = target position + original offset
        Vector3 desiredPos = target.transform.position + initialOffset;

        // Smoothly move toward that position
        transform.position = Vector3.Lerp(
            transform.position,
            desiredPos,
            followSpeed * Time.deltaTime
        );
    }
}
