using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections;

[RequireComponent(typeof(HealthController), typeof(CircleCollider2D))]
public class ShieldController : MonoBehaviour
{
    //private Coroutine _activateShield = null;
    [SerializeField] private float _degrees = 120f;
    private CircleCollider2D _circleCollider2D;
    private bool _shieldActive = false;

    void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;

        Vector3 position;
        SpriteRenderer sprite = GetComponentInChildren<SpriteRenderer>();

        if (sprite != null)
        {
            position = sprite.transform.position;
        }
        else
        {
            position = transform.position;
        }

        if (_shieldActive)
            Gizmos.DrawWireCube(position, Vector3.one * 1f);
    }

    void Start()
    {
        _degrees = Math.Abs(_degrees) % 360;
        _circleCollider2D = GetComponent<CircleCollider2D>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            ToggleShield();
        }
    }

    public void ActivateShield()
    {
        _shieldActive = true;
    }

    public IEnumerator ActivateShield(float duration)
    {
        _shieldActive = true;
        yield return new WaitForSeconds(duration);
        _shieldActive = false;
    }

    public void DeactivateShield()
    {
        _shieldActive = false;
    }

    private bool IsInSector(Vector2 point)
    {
        Vector2 center = _circleCollider2D.transform.TransformPoint(_circleCollider2D.offset);
        Vector2 attackDirection = point - center;

        // Assuming transform.up is "forward" direction
        float angleOffCenter = Vector2.Angle(_circleCollider2D.transform.up, attackDirection);

        return angleOffCenter <= _degrees / 2f;
    }

    public bool ShieldBlocks(Vector2 contactPoint) =>
        _shieldActive && IsInSector(contactPoint);
    

    public void ToggleShield()
    {
        _shieldActive = !_shieldActive;
    }
}
