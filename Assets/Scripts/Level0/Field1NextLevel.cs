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
            player.transform.position = targetPlayerPosition.position;
            // Stop moving when camera is close enough
            if (Vector3.Distance(mainCamera.transform.position, targetCameraPosition.position) < 0.01f)
            {
                mainCamera.transform.position = targetCameraPosition.position;
                moveCamera = false;
                //need to move player to next level
                

            }
        }

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("collision");
        Debug.Log(collision.tag);
        if (collision.CompareTag("Player"))
        {
            Debug.Log("collision with player");
            moveCamera = true;
        }
    }
}
