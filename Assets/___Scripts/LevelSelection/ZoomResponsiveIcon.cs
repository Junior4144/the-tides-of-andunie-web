using UnityEngine;
using UnityEngine.UI;

public class ZoomResponsiveIcon : MonoBehaviour
{
    private Camera _camera;
    private RectTransform _rect;

    [Header("Zoom Settings")]
    public float minZoom = 10f;     // Closest to ground
    public float maxZoom = 340f;    // Farthest view

    [Header("Icon Size (UI)")]
    public float minSize = 5f;      // when zoomed in
    public float maxSize = 25f;     // when zoomed out

    [Header("Fading")]
    public float fadeStart = 15f;
    public float fadeEnd = 5f;

    private CanvasGroup _group;

    private void Awake()
    {
        _rect = GetComponent<RectTransform>();

        _group = GetComponent<CanvasGroup>();
        if (_group == null)
            _group = gameObject.AddComponent<CanvasGroup>();
    }

    private void Start()
    {
        _camera = Camera.main;
    }

    private void Update()
    {
        if (_camera == null) return;

        float zoom = _camera.orthographicSize;

        // Scale icon with zoom
        float t = Mathf.InverseLerp(minZoom, maxZoom, zoom);
        float size = Mathf.Lerp(minSize, maxSize, t);
        _rect.sizeDelta = new Vector2(size, size);

        // Fade with zoom
        float fadeT = Mathf.InverseLerp(fadeEnd, fadeStart, zoom);
        _group.alpha = fadeT;
    }
}
