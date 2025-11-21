using System.Collections.Generic;
using NUnit.Framework;
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
