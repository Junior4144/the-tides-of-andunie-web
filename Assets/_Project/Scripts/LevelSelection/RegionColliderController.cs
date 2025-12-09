using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RegionColliderController : MonoBehaviour
{
    private PolygonOutline[] polygonOutlines;
    private OnClickOutline[] onClickOutlines;
    private LineRenderer[] lineRenderers;
    private MeshRenderer[] meshRenderers;

    private void OnEnable()
    {
        SceneManager.activeSceneChanged += HandleCheck;

        RegionEventBus.OnDisableOfRegionUI += HandleDisablingOfRegionUI;
        RegionEventBus.NoLongerDisableOfRegionUI += HandleNoLongerDisabledUI;

        UIEvents.OnPreScreenConfirm += HandleDisablingOfRegionUI;
        UIEvents.OnPreScreenDeactivated += HandleNoLongerDisabledUI;

        ShopUIController.ShopActivated += HandleDisablingOfRegionUI;
        ShopUIController.ShopDeactivated += HandleNoLongerDisabledUI;

        UIEvents.OnLSEnterConfirm += HandleDisablingOfRegionUI;
        UIEvents.OnLSEnterDeactivated += HandleNoLongerDisabledUI;

        UIEvents.OnPauseMenuActive += HandleDisablingOfRegionUI;
        UIEvents.OnPauseMenuDeactivated += HandleNoLongerDisabledUI;

        UIEvents.DefaultPopUPActive += HandleDisablingOfRegionUI;
        UIEvents.DefaultPopUpDisabled += HandleNoLongerDisabledUI;
    }

    private void OnDisable()
    {
        SceneManager.activeSceneChanged -= HandleCheck;

        RegionEventBus.OnDisableOfRegionUI -= HandleDisablingOfRegionUI;
        RegionEventBus.NoLongerDisableOfRegionUI -= HandleNoLongerDisabledUI;

        UIEvents.OnPreScreenConfirm -= HandleDisablingOfRegionUI;
        UIEvents.OnPreScreenDeactivated -= HandleNoLongerDisabledUI;

        ShopUIController.ShopActivated -= HandleDisablingOfRegionUI;
        ShopUIController.ShopDeactivated -= HandleNoLongerDisabledUI;

        UIEvents.OnLSEnterConfirm -= HandleDisablingOfRegionUI;
        UIEvents.OnLSEnterDeactivated -= HandleNoLongerDisabledUI;

        UIEvents.OnPauseMenuActive -= HandleDisablingOfRegionUI;
        UIEvents.OnPauseMenuDeactivated -= HandleNoLongerDisabledUI;

        UIEvents.DefaultPopUPActive -= HandleDisablingOfRegionUI;
        UIEvents.DefaultPopUpDisabled -= HandleNoLongerDisabledUI;
    }

    private void Awake()
    {
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
        SetAll(true);
        Debug.Log("RegionColliderController ENABLING ALL REGION UI");
    }

    private void SetAll(bool state)
    {
        foreach (var p in polygonOutlines) p.enabled = state;
        foreach (var o in onClickOutlines) o.enabled = state;
        foreach (var l in lineRenderers) l.enabled = state;
        foreach (var m in meshRenderers) m.enabled = state;
    }
}
