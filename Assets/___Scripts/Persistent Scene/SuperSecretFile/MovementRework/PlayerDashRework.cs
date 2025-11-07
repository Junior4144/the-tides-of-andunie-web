using System.Collections;
using UnityEngine;
using UnityEngine.LowLevel;

public class PlayerDash : MonoBehaviour
{
    public float dashDistance = 5f;
    public float dashDuration = 0.12f; // smooth dash timing
    public float dashCooldown = 0.4f;

    private bool canDash = true;
    private Rigidbody2D rb;
    private Camera cam;

    private bool isDashing = false;
    public bool IsDashing => isDashing;

    private Transform playerRoot;

    private PlayerController playerMove;

    private void Awake()
    {
        playerRoot = transform;   // if your rotation happens on parent, assign it instead
        playerMove = GetComponent<PlayerController>();
    }

    private void Start()
    {
        cam = CameraManager.Instance.GetCamera();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1) && canDash && !isDashing)
        {
            Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
            Vector2 dashDir = (mousePos - (Vector2)transform.position).normalized;

            // Rotate to mouse first
            float angle = Mathf.Atan2(dashDir.y, dashDir.x) * Mathf.Rad2Deg - 90f;
            playerRoot.rotation = Quaternion.Euler(0, 0, angle);

            StartCoroutine(Dash(dashDir));
        }
    }

    IEnumerator Dash(Vector2 direction)
    {
        canDash = false;
        isDashing = true;



        Vector2 start = transform.position;
        Vector2 target = start + direction * dashDistance;

        float t = 0f;
        while (t < dashDuration)
        {
            t += Time.deltaTime;
            float p = t / dashDuration;

            transform.position = Vector2.Lerp(start, target, p);
            yield return null;
        }

        isDashing = false;


        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }
}