using UnityEngine;
using UnityEngine.AI;
enum MouseButton { Left = 0, Right = 1, Middle = 2 }

public class CameraLSController : MonoBehaviour
{
    [SerializeField] private float dragSpeed = 5f;
    [SerializeField] private BoxCollider2D boundary;

    [Header("Cursor Sprites")]
    public Texture2D defaultCursor;  
    // same as valid for now
    public Texture2D validClickCursor;
    public Texture2D invalidClickCursor;

    public Texture2D dragCursor;
    public Vector2 hotspot = Vector2.zero;

    Camera cam;
    Vector3 lastMousePos;
    Transform PlayerTransform;
    Bounds bounds;

    void Start()
    {
        PlayerTransform = PlayerManager.Instance.GetPlayerTransform();
        cam = CameraManager.Instance.GetCamera();
        bounds = boundary.bounds;

        transform.position = new Vector3(PlayerTransform.position.x, PlayerTransform.position.y, transform.position.z);
        Cursor.SetCursor(defaultCursor, hotspot, CursorMode.Auto);
    }

    void Update()
    {
        HandleDrag(MouseButton.Left);
        HandleDrag(MouseButton.Middle);
        HandleRightClickNav();
    }
    void HandleDrag(MouseButton button)
    {
        if (Input.GetMouseButtonDown((int)button))
        {
            lastMousePos = cam.ScreenToWorldPoint(Input.mousePosition);
            Cursor.SetCursor(dragCursor, hotspot, CursorMode.Auto);
        }

        if (Input.GetMouseButton((int)button))
        {
            Vector3 currentMousePos = cam.ScreenToWorldPoint(Input.mousePosition);
            Vector3 diff = lastMousePos - currentMousePos;

            Vector3 newPos = transform.position + diff * dragSpeed;
            newPos.x = Mathf.Clamp(newPos.x, bounds.min.x, bounds.max.x);
            newPos.y = Mathf.Clamp(newPos.y, bounds.min.y, bounds.max.y);

            transform.position = newPos;
        }

        if (Input.GetMouseButtonUp((int)button))
        {
            Cursor.SetCursor(defaultCursor, hotspot, CursorMode.Auto);
        }
    }

    void HandleRightClickNav()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Vector2 mouseWorld = cam.ScreenToWorldPoint(Input.mousePosition);
            bool valid = NavMesh.SamplePosition(mouseWorld, out _, 0.3f, NavMesh.AllAreas);

            if (valid)
            {
                Cursor.SetCursor(validClickCursor, hotspot, CursorMode.Auto);
            }
            else
            {
                Cursor.SetCursor(invalidClickCursor, hotspot, CursorMode.Auto);
            }
        }

        if (Input.GetMouseButtonUp(1))
        {
            Cursor.SetCursor(defaultCursor, hotspot, CursorMode.Auto);
        }
    }

}
