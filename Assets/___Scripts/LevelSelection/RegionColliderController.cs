using UnityEngine;

public class RegionColliderController : MonoBehaviour
{
    private PolygonOutline[] polygonOutlines;
    private OnClickOutline[] onClickOutlines;
    private LineRenderer[] lineRenderers;
    private MeshRenderer[] meshRenderers;

    private void OnEnable()
    {
        RegionZoomController.OnDisableOfRegionUI += HandleDisablingOfRegionUI;
        RegionZoomController.NoLongerDisableOfRegionUI += HandleNoLongerDisabledUI;

        UIEvents.OnPreScreenConfirm += HandleDisablingOfRegionUI;
        UIEvents.OnPreScreenDeactivated += HandleNoLongerDisabledUI;
    }

    private void OnDisable()
    {
        RegionZoomController.OnDisableOfRegionUI -= HandleDisablingOfRegionUI;
        RegionZoomController.NoLongerDisableOfRegionUI -= HandleNoLongerDisabledUI;

        UIEvents.OnPreScreenConfirm -= HandleDisablingOfRegionUI;
        UIEvents.OnPreScreenDeactivated -= HandleNoLongerDisabledUI;
    }

    private void Start()
    {
        // Collect all matching components in children (including inactive)
        polygonOutlines = GetComponentsInChildren<PolygonOutline>(true);
        onClickOutlines = GetComponentsInChildren<OnClickOutline>(true);
        lineRenderers = GetComponentsInChildren<LineRenderer>(true);
        meshRenderers = GetComponentsInChildren<MeshRenderer>(true);

        if (!LSManager.Instance.HasInvasionStarted)
            gameObject.SetActive(false);
    }

    private void HandleDisablingOfRegionUI()
    {
        SetAll(false);
        Debug.Log("RegionColliderController DISABLING ALL REGION UI");
    }

    private void HandleNoLongerDisabledUI()
    {
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
