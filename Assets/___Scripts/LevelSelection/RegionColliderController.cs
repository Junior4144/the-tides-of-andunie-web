using System.Collections.Generic;
using UnityEngine;

public class RegionColliderController : MonoBehaviour
{
    [SerializeField] private List<GameObject> list = new();

    private void OnEnable()
    {
        RegionZoomController.OnDisableOfRegionUI += HandleDisablingOfRegionUI;
        RegionZoomController.NoLongerDisableOfRegionUI += HandleNoLongerDisabledUI;
    }
    private void OnDisable()
    {
        RegionZoomController.OnDisableOfRegionUI -= HandleDisablingOfRegionUI;
        RegionZoomController.NoLongerDisableOfRegionUI -= HandleNoLongerDisabledUI;
    }
    private void Start()
    {
        if(!LSManager.Instance.HasInvasionStarted) gameObject.SetActive(false);
    }

    private void HandleDisablingOfRegionUI()
    {
        foreach (GameObject go in list)
        {
            go.SetActive(false);
        }
    }

    private void HandleNoLongerDisabledUI()
    {
        foreach (GameObject go in list)
        {
            go.SetActive(true);
        }
    }
}
