using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float speed = 5f; // Player movement speed

    [SerializeField] private AttackRework attackScript;

    [SerializeField] private float rotationSpeed = 8f;       // Base speed
    [SerializeField] private float startTurnBoost = 1.3f;    // Increase speed when starting a turn
    [SerializeField] private float endTurnDamp = 0.85f;      // Slow just before finishing
    [SerializeField] private float minTurnSpeed = 4f;        // Never rotate slower than this
    [SerializeField] private float rotationSnapBuffer = 0.12f; // anti-spam delay
    private float lastTurnTime = 0f;

    private bool movementEnabled = true;

    public void SetMovementEnabled(bool enabled)
    {
        movementEnabled = enabled;
    }

    void Update()
    {
        if (!movementEnabled) return;

        // Detect keys
        bool up = Input.GetKey(KeyCode.W);
        bool down = Input.GetKey(KeyCode.S);
        bool left = Input.GetKey(KeyCode.A);
        bool right = Input.GetKey(KeyCode.D);

        // Fix diagonal speed (1 / √2 = 0.7071)
        float diagMultiplier = 1f;
        if ((up || down) && (left || right))
        {
            diagMultiplier = 0.7071f;
        }

        // Movement (world-space to avoid rotation affecting WASD direction)
        if (up)
            transform.Translate(Vector2.up * testVert(Vector2.up) * diagMultiplier, Space.World);

        if (left)
            transform.Translate(Vector2.left * testHor(Vector2.left) * diagMultiplier, Space.World);

        if (down)
            transform.Translate(Vector2.down * testVert(Vector2.down) * diagMultiplier, Space.World);

        if (right)
            transform.Translate(Vector2.right * testHor(Vector2.right) * diagMultiplier, Space.World);
    }

    private float testHor(Vector2 dir)
    {
        Vector2 origin = transform.position;
        float offset;
        if (dir.Equals(Vector2.left))
        {
            offset = -0.125f; // Offset for left side of character
        }
        else
        {
            offset = 0.125f; // Offset for right side of character
        }
        origin.x += offset;
        RaycastHit2D raycast = Physics2D.Raycast(origin, dir, speed * Time.deltaTime);

        if (raycast.collider != null && raycast.collider.gameObject.tag == "Collidable")
        { // If raycast hits something tagged "Collidable"
            float distance = Math.Abs(raycast.point.x - origin.x);
            return distance;
        }

        return speed * Time.deltaTime;
    }

    private float testVert(Vector2 dir)
    {
        Vector2 origin = transform.position;
        float offset;
        if (dir.Equals(Vector2.up))
        {
            offset = .1f; // Offset for top side of character
        }
        else
        {
            offset = -0.5f; // Offset for bottom side of character
        }
        origin.y += offset;
        RaycastHit2D raycast = Physics2D.Raycast(origin, dir, speed * Time.deltaTime);

        if (raycast.collider != null && raycast.collider.gameObject.tag == "Collidable")
        { // If raycast hits something tagged "Collidable"
            float distance = Math.Abs(raycast.point.y - origin.y);
            return distance;
        }

        return speed * Time.deltaTime;
    }

    private void FixedUpdate()
    {
        if (attackScript.IsAttacking) return;
            RotatePlayerEightWay();
    }

    private void RotatePlayerEightWay()
    {
        if (Time.time - lastTurnTime < rotationSnapBuffer)
            return;

        bool up = Input.GetKey(KeyCode.W);
        bool down = Input.GetKey(KeyCode.S);
        bool left = Input.GetKey(KeyCode.A);
        bool right = Input.GetKey(KeyCode.D);

        if (!up && !down && !left && !right) return;

        float targetAngle = transform.eulerAngles.z;
        lastTurnTime = Time.time;

        // 8-direction target angles
        if (up && right) targetAngle = -45f;
        else if (up && left) targetAngle = 45f;
        else if (down && right) targetAngle = -135f;
        else if (down && left) targetAngle = 135f;
        else if (up) targetAngle = 0f;
        else if (down) targetAngle = 180f;
        else if (right) targetAngle = -90f;
        else if (left) targetAngle = 90f;

        // Calculate angle difference (0–180 degrees)
        float angleDiff = Mathf.DeltaAngle(transform.eulerAngles.z, targetAngle);

        // Dynamic turn speed based on angle distance
        float dynamicSpeed = rotationSpeed;

        // Boost at start of turn (snappy feel)
        if (Mathf.Abs(angleDiff) > 60f)
            dynamicSpeed *= startTurnBoost;

        // Damp near finish (ease-out)
        if (Mathf.Abs(angleDiff) < 20f)
            dynamicSpeed *= endTurnDamp;

        // Minimum turn speed
        dynamicSpeed = Mathf.Max(dynamicSpeed, minTurnSpeed);

        // Smooth rotate
        Quaternion targetRot = Quaternion.Euler(0, 0, targetAngle);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, dynamicSpeed * Time.deltaTime);
    }
}
