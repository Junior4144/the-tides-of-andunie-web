using System.Collections;
using UnityEngine;
using Unity.Cinemachine;

public class CameraSlide : MonoBehaviour
{
    [Header("Camera Settings")]
    [SerializeField] private CinemachineCamera virtualCamera;
    [SerializeField] private Transform targetPoint;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float waitTime = 2f;

    private Transform _cameraTransform;
    private Vector3 _originalPosition;

    private void Start()
    {
        if (virtualCamera == null)
            virtualCamera = GetComponent<CinemachineCamera>();

        _originalPosition = _cameraTransform.position;
    }

    public void SlideCamera() =>
        StartCoroutine(MoveCameraSequence());

    private IEnumerator MoveCameraSequence()
    {
        yield return MoveCameraToPosition(targetPoint.position);
        yield return new WaitForSeconds(waitTime);
        yield return MoveCameraToPosition(_originalPosition);
    }

    private IEnumerator MoveCameraToPosition(Vector3 target)
    {
        Vector3 adjustedTarget = new Vector3(target.x, target.y, _cameraTransform.position.z);
        while (Vector3.Distance(_cameraTransform.position, adjustedTarget) > 0.01f)
        {
            _cameraTransform.position = Vector3.Lerp(
                _cameraTransform.position,
                adjustedTarget,
                moveSpeed * Time.deltaTime
            );
            yield return null;
        }
        _cameraTransform.position = adjustedTarget;
    }
}