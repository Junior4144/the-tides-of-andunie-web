using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class ZoomResponsiveLabel : MonoBehaviour
{
    private TextMeshProUGUI _text;
    private Camera _camera;

    [Header("Zoom Settings")]
    public float minZoom = 10f;    // Closest zoom level
    public float maxZoom = 340f;   // Farthest zoom level

    [Header("Scaling")]
    public float minFontSize = 20f;
    public float maxFontSize = 200f;

    [Header("Fading")]
    public float fadeStart = 15f; // When fading starts (zoom level)
    public float fadeEnd = 5f;    // When fully transparent (zoomed in)

    private CanvasGroup _group;

    private void Awake()
    {
        _text = GetComponent<TextMeshProUGUI>();

        _group = GetComponent<CanvasGroup>();
        if (_group == null)
            _group = gameObject.AddComponent<CanvasGroup>();
    }
    private void Start()
    {
        _camera = CameraManager.Instance != null ? CameraManager.Instance.GetCamera() : Camera.main;
    }

    private void Update()
    {
        if (_camera == null) return;

        float zoom = _camera.orthographicSize;

        // 1️⃣ Scale font with zoom
        float t = Mathf.InverseLerp(minZoom, maxZoom, zoom);
        _text.fontSize = Mathf.Lerp(minFontSize, maxFontSize, t);

        // 2️⃣ Fade based on zoom
        float fadeT = Mathf.InverseLerp(fadeEnd, fadeStart, zoom);
        _group.alpha = fadeT;
    }
}
