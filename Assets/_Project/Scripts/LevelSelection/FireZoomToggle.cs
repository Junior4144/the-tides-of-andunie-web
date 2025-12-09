using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FireZoomToggle : MonoBehaviour
{
    private Camera cam;
    public float zoomThreshold = 100f;

    GameObject[] fireObjects;
    bool lastState = true;
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
        {
            CacheAnimation();
        }

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
