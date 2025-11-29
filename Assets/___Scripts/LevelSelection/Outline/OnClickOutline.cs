using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

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
    private void OnEnable() => SceneManager.activeSceneChanged += HandleCheck;

    private void OnDisable() => SceneManager.activeSceneChanged -= HandleCheck;

    private void HandleCheck(Scene oldScene, Scene newScene)
    {
        StartCoroutine(CheckAfterLoading(newScene));
    }

    private IEnumerator CheckAfterLoading(Scene newScene)
    {
        yield return null;

        if (newScene == gameObject.scene)
            HandleSetup();
    }

    private void HandleSetup()
    {
        if (LSRegionLockManager.Instance.IsRegionLocked(region))
        {
            isInvaded = true;
        }
        ResetColor();
    }

    private void Update()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButtonDown(0) && wasHovering)
        {
            bool validNavMesh = NavMesh.SamplePosition(mousePos, out NavMeshHit navHit, 0.5f, NavMesh.AllAreas);

            Collider2D[] hits = Physics2D.OverlapPointAll(mousePos);

            bool hasPointerCollider = false;

            foreach (var col in hits)
            {
                if (col.TryGetComponent<VillagePointerTargetController>(out _))
                {
                    hasPointerCollider = true;
                    break;
                }
            }

            if (!validNavMesh && !hasPointerCollider)
            {
                Debug.Log($"[OnClickOutline] Region Clicked: {region}");
                RegionClicked?.Invoke(region);
            }
            else
            {
                Debug.Log("[OnClickOutline] Ignored click (navmesh or collider present)");
            }
        }

        // --- HOVER CHECK ---
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