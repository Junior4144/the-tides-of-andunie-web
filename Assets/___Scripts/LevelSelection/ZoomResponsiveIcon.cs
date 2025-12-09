using UnityEngine;
using UnityEngine.UI;

public class ZoomResponsiveIcon : MonoBehaviour
{
    private Camera _camera;
    private RectTransform _rect;

    [Header("Zoom Settings")]
    public float minZoom = 10f;
    public float maxZoom = 340f;

    [Header("Icon Width (UI)")]
    public float minWidth = 10f;
    public float maxWidth = 20f;

    [Header("Icon Height (UI)")]
    public float minHeight = 7f;
    public float maxHeight = 14f;

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
        
        float t = Mathf.InverseLerp(minZoom, maxZoom, zoom);
        float width = Mathf.Lerp(minWidth, maxWidth, t);
        float height = Mathf.Lerp(minHeight, maxHeight, t);
        _rect.sizeDelta = new Vector2(width, height);
        
        float fadeT = Mathf.InverseLerp(fadeEnd, fadeStart, zoom);
        _group.alpha = fadeT;
    }
}
