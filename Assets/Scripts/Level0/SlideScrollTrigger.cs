using UnityEngine;

public class SlideScrollTrigger : MonoBehaviour
{

    public Transform targetCameraPosition; // Drag the new camera position here in Inspector
    public float moveSpeed = 2f;           // Adjust speed of camera movement
    private bool moveCamera = false;

    private Camera mainCamera;
    public Transform targetPlayerPosition;

    [SerializeField]
    public GameObject player;

    private void Start()
    {
        mainCamera = Camera.main;
    }
    private void Update()
    {
        if (moveCamera)
        {
            
            mainCamera.transform.position = Vector3.Lerp(
                mainCamera.transform.position,
                targetCameraPosition.position,
                moveSpeed * Time.deltaTime
            );
            
            player.transform.position = targetPlayerPosition.position;

            if (Vector3.Distance(mainCamera.transform.position, targetCameraPosition.position) < .01f)
            {
                mainCamera.transform.position = targetCameraPosition.position;
                moveCamera = false;

            }
        }

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Player"))
        {
            moveCamera = true;
        }
    }
}
