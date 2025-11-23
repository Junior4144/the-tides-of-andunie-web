using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class OnClickOutline : MonoBehaviour
{
    [SerializeField] private Region region;
    [SerializeField] private Color hoverColor = Color.yellow;
    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color invadedColor = Color.red;


    private LineRenderer lineRenderer;
    private Collider2D col;

    public static event Action<Region> RegionClicked;

    private bool wasHovering = false;

    private bool isInvaded = false;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        col = GetComponent<Collider2D>();

        if (lineRenderer != null)
        {
            lineRenderer.startColor = normalColor;
            lineRenderer.endColor = normalColor;
        }
        
    }
    private void Start()
    {
        if (LSRegionLockManager.Instance.IsRegionLocked(region))
        {
            isInvaded = true;
        }
    }
    private void Update()
    {
        // --- CLICK ---
        if (Input.GetMouseButtonDown(0) && wasHovering)
        {
            Debug.Log($"[OnClickOutline] Region Clicked: {region}");
            RegionClicked?.Invoke(region);
        }

        // --- HOVER CHECK ---
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        bool isHovering = col.OverlapPoint(mousePos);

        if (isHovering && !wasHovering)
            OnPointerEnter();
        else if (!isHovering && wasHovering)
            OnPointerExit();

        wasHovering = isHovering;
    }

    private void OnPointerEnter()
    {
        Debug.Log($"[OnClickOutline] Hover Enter ({region})");

        if (lineRenderer != null)
        {
            lineRenderer.startColor = hoverColor;
            lineRenderer.endColor = hoverColor;
            lineRenderer.widthMultiplier = 1.5f;
            lineRenderer.sortingOrder = 11;
        }
    }

    private void OnPointerExit()
    {
        Debug.Log($"[OnClickOutline] Hover Exit ({region})");

        if (lineRenderer == null) return;

        if (isInvaded)
        {
            lineRenderer.startColor = invadedColor;
            lineRenderer.endColor = invadedColor;
            lineRenderer.widthMultiplier = 1f;
            lineRenderer.sortingOrder = 1;
            return;
        }

        lineRenderer.startColor = normalColor;
        lineRenderer.endColor = normalColor;
        lineRenderer.sortingOrder = 3;

    }

    public void ResetColor()
    {
        if (lineRenderer == null) return;

        if (isInvaded)
        {
            lineRenderer.startColor = invadedColor;
            lineRenderer.endColor = invadedColor;
            lineRenderer.widthMultiplier = 1f;
            lineRenderer.sortingOrder = 1;
            return;
        }

        lineRenderer.startColor = normalColor;
        lineRenderer.endColor = normalColor;
        lineRenderer.sortingOrder = 3;
    }
}