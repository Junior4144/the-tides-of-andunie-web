using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RegionZoomController : MonoBehaviour
{
    public float threshold = 200f;
    public float PreInvasionThreshold = 400f;

    private Camera cam;
    private LSPlayerMovement playerMovement;
    private bool _activate = false;


    public static event Action ZoomBelowThreshold;
    public static event Action ZoomAboveThreshold;

    public static event Action OnDisableOfRegionUI;
    public static event Action NoLongerDisableOfRegionUI;


    private void OnEnable() => SceneManager.activeSceneChanged += HandleCheck;

    private void OnDisable() => SceneManager.activeSceneChanged -= HandleCheck;

    private void Start()
    {
        cam = CameraManager.Instance.GetCamera();
        playerMovement = PlayerManager.Instance.gameObject.GetComponent<LSPlayerMovement>();

        if(!LSManager.Instance.HasInvasionStarted) threshold = PreInvasionThreshold;
    }

    private void Update()
    {
        if (!_activate) return;

        if (cam.orthographicSize <= threshold)
        {
            if (playerMovement == null) return;
            playerMovement.enabled = true;
            ZoomBelowThreshold?.Invoke();
            OnDisableOfRegionUI?.Invoke();
        }

        if (cam.orthographicSize > threshold)
        {
            if (playerMovement == null) return;
            playerMovement.enabled = false;
            ZoomAboveThreshold?.Invoke();
            NoLongerDisableOfRegionUI?.Invoke();
        }
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