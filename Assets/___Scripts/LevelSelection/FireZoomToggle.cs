using System.Collections;
using UnityEngine;

public class FireZoomToggle : MonoBehaviour
{
    private Camera cam;
    public float zoomThreshold = 100f;

    GameObject[] fireObjects;
    bool lastState = true;

    private IEnumerator Start()
    {
        yield return null;
        fireObjects = GameObject.FindGameObjectsWithTag("FireAnimation");
        cam = CameraManager.Instance.GetCamera();
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
