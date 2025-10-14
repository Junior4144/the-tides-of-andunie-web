using UnityEngine;

public class FollowCameraTarget : MonoBehaviour
{
    [SerializeField] private CameraTarget target;
    [SerializeField] private float followSpeed = 5f; 

    private Vector3 initialOffset;

    void Start()
    {
        if (target != null)
            initialOffset = transform.position - target.transform.position;
    }

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPos = new Vector3(
            target.transform.position.x + initialOffset.x,
            transform.position.y,
            transform.position.z
        );

        transform.position = Vector3.Lerp(
            transform.position,
            desiredPos,
            followSpeed * Time.deltaTime
        );
    }

}
