using UnityEngine;
using UnityEngine.EventSystems;
using System;

public enum Region
{
    Orrostar,
    Hyarrostar,
    Hyarnustar,
    Andustar,
    Forostar,
    None,
}

public class OnClickOutline : MonoBehaviour
{
    [SerializeField] private Region region;
    [SerializeField] private Color hoverColor = Color.yellow;
    [SerializeField] private Color normalColor = Color.white;

    private LineRenderer lineRenderer;
    private Collider2D col;

    public static event Action<Region> RegionClicked;

    private bool wasHovering = false;

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
        }
    }

    private void OnPointerExit()
    {
        Debug.Log($"[OnClickOutline] Hover Exit ({region})");

        if (lineRenderer != null)
        {
            lineRenderer.startColor = normalColor;
            lineRenderer.endColor = normalColor;
            lineRenderer.widthMultiplier = 1f;
        }
    }
}