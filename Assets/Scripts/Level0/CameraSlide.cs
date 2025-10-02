using System.Collections;
using UnityEngine;

public class CameraSlide : MonoBehaviour
{
    [Header("Camera Settings")]
    [SerializeField] private Transform targetPoint;
    [SerializeField] private float moveSpeed; 
    [SerializeField] private float waitTime;

    private Vector3 _originalPosition;

    private void Start() =>
        _originalPosition = transform.position; 

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
        while (Vector3.Distance(transform.position, target) > 0.01f)
        {
            transform.position = Vector3.Lerp(transform.position, target, moveSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = target;
    }
}
