using UnityEngine;

public class Field1NextLevel : MonoBehaviour
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
        Debug.Log("collision");
        mainCamera = Camera.main;

    }
    private void Update()
    {
        if (moveCamera)
        {
            // Smoothly move camera towards target
            mainCamera.transform.position = Vector3.Lerp(
                mainCamera.transform.position,
                targetCameraPosition.position,
                moveSpeed * Time.deltaTime
            );
            // Move player object
            player.transform.position = targetPlayerPosition.position;

            // Stop moving when camera is close enough
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
