using UnityEngine;

public class CannonSpawnerMovement : MonoBehaviour
{
    public Camera _camera;
    public Vector2 offset = new Vector2(1f, 0f);
    private float zDistanceToCamera;
    void Start()
    {
        _camera = Camera.main;
        zDistanceToCamera = Mathf.Abs(_camera.transform.position.z - transform.position.z);
    }
    void LateUpdate()
    {
        Vector3 rightEdgeCamera = _camera.ViewportToWorldPoint(
            new Vector3(1f, 0.5f, zDistanceToCamera)
        );
        ChangeSpawnerPosition(rightEdgeCamera);
    }
    private void ChangeSpawnerPosition(Vector3 rightEdgeCamera)
    {
        transform.position = new Vector3(
        rightEdgeCamera.x + offset.x,
        transform.position.y,
        transform.position.z
        );
    }

}

