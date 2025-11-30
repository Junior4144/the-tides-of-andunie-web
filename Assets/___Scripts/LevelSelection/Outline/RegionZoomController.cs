using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RegionZoomController : MonoBehaviour
{
    public float threshold = 200f;
    public float PreInvasionThreshold = 400f;

    private Camera cam;
    private bool _activate = false;
    private bool wasBelowThreshold = false;

    public static event Action ZoomBelowThreshold;
    public static event Action ZoomAboveThreshold;

    public static event Action OnDisableOfRegionUI;
    public static event Action NoLongerDisableOfRegionUI;


    private void OnEnable() => SceneManager.activeSceneChanged += HandleCheck;

    private void OnDisable() => SceneManager.activeSceneChanged -= HandleCheck;

    private void Start()
    {
        cam = CameraManager.Instance.GetCamera();

        if(!LSManager.Instance.HasInvasionStarted) threshold = PreInvasionThreshold;

        wasBelowThreshold = cam.orthographicSize <= threshold;
    }

    private void Update()
    {
        if (!_activate) return;

        bool isBelowThreshold = cam.orthographicSize <= threshold;

        // Only fire events when crossing from above -> below
        if (isBelowThreshold && !wasBelowThreshold)
        {
            ZoomBelowThreshold?.Invoke();
            OnDisableOfRegionUI?.Invoke();
        }

        // Only fire events when crossing from below -> above
        if (!isBelowThreshold && wasBelowThreshold)
        {
            ZoomAboveThreshold?.Invoke();
            NoLongerDisableOfRegionUI?.Invoke();
        }

        // Update previous state
        wasBelowThreshold = isBelowThreshold;
    }

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
        _activate = true;
    }
}