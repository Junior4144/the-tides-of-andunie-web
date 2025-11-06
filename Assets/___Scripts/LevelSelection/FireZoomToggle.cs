using System.Collections;
using UnityEngine;

public class FireZoomToggle : MonoBehaviour
{
    private Camera cam;
    public float zoomThreshold = 100f;

    GameObject[] fireObjects;
    bool lastState = true;
    private void OnEnable()
    {
        InSceneActivationManager.OnSceneActivated += CacheAnimation;
    }

    private void OnDisable()
    {
        InSceneActivationManager.OnSceneActivated -= CacheAnimation;
    }
    private void Start()
    {
        cam = CameraManager.Instance.GetCamera();
    }

    private void CacheAnimation()
    {
        StartCoroutine(StartingCacheAnimation());
    }

    private IEnumerator StartingCacheAnimation()
    {
        yield return null;
        fireObjects = GameObject.FindGameObjectsWithTag("FireAnimation");
        
    }

    void Update()
    {
        bool show = cam.orthographicSize <= zoomThreshold;
        if (show == lastState) return;

        lastState = show;

        foreach (var f in fireObjects)
            f.SetActive(show);
    }
}
