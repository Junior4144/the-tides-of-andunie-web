using UnityEngine;

public class Utility : MonoBehaviour
{
    public static float AngleTowardsMouse(Vector3 pos)
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = -10f;

        Vector3 objectPos = Camera.main.WorldToScreenPoint(pos);
        mousePos.x = mousePos.x - objectPos.x;
        mousePos.y = mousePos.y - objectPos.y; 

        float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg - 90f;

        return angle;
    }
    public static Vector2 DirectionTowardsMouse(Vector3 worldPos)
    {
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return ((Vector2)mouseWorld - (Vector2)worldPos).normalized;
    }

    public static Quaternion RotationTowardsMouse(Vector3 fromPosition)
    {
        float angle = AngleTowardsMouse(fromPosition);
        return Quaternion.Euler(0f, 0f, angle);
    }
}
