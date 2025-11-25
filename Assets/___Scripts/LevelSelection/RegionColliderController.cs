using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RegionColliderController : MonoBehaviour
{
    private PolygonOutline[] polygonOutlines;
    private OnClickOutline[] onClickOutlines;
    private LineRenderer[] lineRenderers;
    private MeshRenderer[] meshRenderers;

    private bool canShowUI = false;   // gate controlled by zoom level

    private void OnEnable()
    {
        SceneManager.activeSceneChanged += HandleCheck;

        RegionZoomController.OnDisableOfRegionUI += HandleDisablingOfRegionUI;
        RegionZoomController.NoLongerDisableOfRegionUI += HandleNoLongerDisabledUI;

        RegionZoomController.ZoomBelowThreshold += DisableZoomUI;
        RegionZoomController.ZoomAboveThreshold += EnableZoomUI;

        UIEvents.OnPreScreenConfirm += HandleDisablingOfRegionUI;
        UIEvents.OnPreScreenDeactivated += HandleNoLongerDisabledUI;

        ShopUIController.ShopActivated += HandleDisablingOfRegionUI;
        ShopUIController.ShopDeactivated += HandleNoLongerDisabledUI;

        UIEvents.OnLSEnterConfirm += HandleDisablingOfRegionUI;
        UIEvents.OnLSEnterDeactivated += HandleNoLongerDisabledUI;

        UIEvents.OnPauseMenuActive += HandleDisablingOfRegionUI;
        UIEvents.OnPauseMenuDeactivated += HandleNoLongerDisabledUI;
    }

    private void OnDisable()
    {
        SceneManager.activeSceneChanged -= HandleCheck;

        RegionZoomController.OnDisableOfRegionUI -= HandleDisablingOfRegionUI;
        RegionZoomController.NoLongerDisableOfRegionUI -= HandleNoLongerDisabledUI;

        RegionZoomController.ZoomBelowThreshold -= DisableZoomUI;
        RegionZoomController.ZoomAboveThreshold -= EnableZoomUI;

        UIEvents.OnPreScreenConfirm -= HandleDisablingOfRegionUI;
        UIEvents.OnPreScreenDeactivated -= HandleNoLongerDisabledUI;

        ShopUIController.ShopActivated -= HandleDisablingOfRegionUI;
        ShopUIController.ShopDeactivated -= HandleNoLongerDisabledUI;

        UIEvents.OnLSEnterConfirm -= HandleDisablingOfRegionUI;
        UIEvents.OnLSEnterDeactivated -= HandleNoLongerDisabledUI;

        UIEvents.OnPauseMenuActive -= HandleDisablingOfRegionUI;
        UIEvents.OnPauseMenuDeactivated -= HandleNoLongerDisabledUI;
    }

    private void Awake()
    {
        // Collect all matching components in children (including inactive)
        polygonOutlines = GetComponentsInChildren<PolygonOutline>(true);
        onClickOutlines = GetComponentsInChildren<OnClickOutline>(true);
        lineRenderers = GetComponentsInChildren<LineRenderer>(true);
        meshRenderers = GetComponentsInChildren<MeshRenderer>(true);

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
        if (!LSManager.Instance.HasInvasionStarted)
            gameObject.SetActive(false);

        canShowUI = false;
        SetAll(false);
    }


    private void DisableZoomUI()
    {
        canShowUI = false;
    }

    private void EnableZoomUI()
    {
        canShowUI = true;
    }

    private void HandleDisablingOfRegionUI()
    {
        SetAll(false);
        Debug.Log("RegionColliderController DISABLING ALL REGION UI");
    }

    private void HandleDisablingOfRegionUI(bool isExit)
    {
        SetAll(false);
        Debug.Log("RegionColliderController DISABLING ALL REGION UI");
    }

    private void HandleNoLongerDisabledUI()
    {
        if (canShowUI)
            SetAll(true);
    }

    private void SetAll(bool state)
    {
        foreach (var p in polygonOutlines) p.enabled = state;
        foreach (var o in onClickOutlines) o.enabled = state;
        foreach (var l in lineRenderers) l.enabled = state;
        foreach (var m in meshRenderers) m.enabled = state;
    }
}
