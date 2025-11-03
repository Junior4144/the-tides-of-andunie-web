using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class ZoomResponsiveLabel : MonoBehaviour
{
    private TextMeshProUGUI _text;
    private Camera _camera;

    [Header("Zoom Settings")]
    public float minZoom = 10f;
    public float maxZoom = 340f;

    [Header("Scaling")]
    public float minFontSize = 20f;
    public float maxFontSize = 200f;

    [Header("Panel Height Scale")]
    public RectTransform panelRect; // ✅ Assign your panel here
    public float minHeight = 6f;
    public float maxHeight = 30f;

    [Header("Fading")]
    public float fadeStart = 15f;
    public float fadeEnd = 5f;

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

        // Zoom ratio (0 = closest, 1 = farthest)
        float t = Mathf.InverseLerp(minZoom, maxZoom, zoom);

        // Scale font
        _text.fontSize = Mathf.Lerp(minFontSize, maxFontSize, t);

        // ✅ Scale Panel Height
        if (panelRect != null)
        {
            float targetHeight = Mathf.Lerp(minHeight, maxHeight, t);
            panelRect.sizeDelta = new Vector2(panelRect.sizeDelta.x, targetHeight);
        }

        // Fade
        float fadeT = Mathf.InverseLerp(fadeEnd, fadeStart, zoom);
        _group.alpha = fadeT;
    }
}
