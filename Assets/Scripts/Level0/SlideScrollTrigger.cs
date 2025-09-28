using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class SlideScrollTrigger : MonoBehaviour
{
    private Camera mainCamera;
    private bool moveCamera = false;

    [Header("Camera Settings")]
    public Transform targetCameraPosition;
    public Transform targetPlayerPosition;


    [SerializeField]
    public float moveSpeed;
    

    [Header("Player Settings")]
    [SerializeField] private float arrivalThreshold = 0.01f;
    [SerializeField] public GameObject player;


    public EnemyManager enemyManager;

    private void Start()
    {
        mainCamera = Camera.main;
    }
    private void Update()
    {
        if (moveCamera)
        {
            HandleCameraMovement();
            
        }
    }

    private void HandleCameraMovement()
    {
        MoveCameraTowardsTarget();
        MovePlayerToTarget();

        if (HasCameraReachedTarget())
        {
            CompleteCameraTransition();
            DestroyAllEnemies();
        }
    }
    private void DestroyAllEnemies()
    {
        enemyManager.DestroyAllEnemies();
        Destroy(gameObject);
    }
    private void MoveCameraTowardsTarget()
    {
        mainCamera.transform.position = Vector3.Lerp(
            mainCamera.transform.position,
            targetCameraPosition.position,
            moveSpeed * Time.deltaTime
        );
    }
    private void MovePlayerToTarget()
    {
        player.transform.position = targetPlayerPosition.position;
    }
    private bool HasCameraReachedTarget()
    {
        return Vector3.Distance(
            mainCamera.transform.position,
            targetCameraPosition.position
        ) < arrivalThreshold;
    }
    private void CompleteCameraTransition()
    {
        SnapCameraToTarget();
        moveCamera = false;
    }
    private void SnapCameraToTarget()
    {
        mainCamera.transform.position = targetCameraPosition.position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Player"))
        {
            moveCamera = true;
        }
    }
}
