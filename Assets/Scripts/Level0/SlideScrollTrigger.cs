using UnityEngine;
using System.Collections;

public class CameraSlide : MonoBehaviour
{
    [Header("Camera Settings")]
    [SerializeField] private Transform targetPoint;
    [SerializeField] private float moveSpeed; 
    [SerializeField] private float waitTime;

    private Vector3 originalPosition;

    private void Start()
    {
        originalPosition = transform.position; 
    }
    public void SlideCamera() =>
        StartCoroutine(MoveCameraSequence());
    private IEnumerator MoveCameraSequence()
    {
        while (Vector3.Distance(transform.position, targetPoint.position) > .01f)
        {
            transform.position = Vector3.Lerp(transform.position, targetPoint.position, moveSpeed * Time.deltaTime);
            yield return null;
        }

        yield return new WaitForSeconds(waitTime);

        while (Vector3.Distance(transform.position, originalPosition) > .01f)
        {
            transform.position = Vector3.Lerp(transform.position, originalPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }
    }
}
