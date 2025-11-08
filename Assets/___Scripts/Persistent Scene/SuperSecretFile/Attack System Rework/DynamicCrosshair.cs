using UnityEngine;
using UnityEngine.UI;

public class DynamicCrosshair : MonoBehaviour
{
    public RectTransform crosshairRoot;
    public RectTransform top, bottom, left, right;
    [Header("Settings")]
    public float idleSpread = 40f;
    public float aimSpread = 10f;
    public float moveSpeed = 10f;

    private float currentSpread;

    void OnEnable()
    {
        Cursor.lockState = CursorLockMode.None;
        currentSpread = idleSpread;
        Cursor.visible = false;
    }
    private void OnDisable()
    {
        Cursor.visible = true;
    }

    void Update()
    {
        // 🧭 Move crosshair to mouse position
        crosshairRoot.position = Input.mousePosition;

        // 🎯 Shrink when aiming
        float targetSpread = Input.GetMouseButton(0) ? aimSpread : idleSpread;
        currentSpread = Mathf.Lerp(currentSpread, targetSpread, Time.deltaTime * moveSpeed);

        // 🧩 Update child positions
        top.localPosition = new Vector3(0, currentSpread, 0);
        bottom.localPosition = new Vector3(0, -currentSpread, 0);
        left.localPosition = new Vector3(-currentSpread, 0, 0);
        right.localPosition = new Vector3(currentSpread, 0, 0);
    }
}
