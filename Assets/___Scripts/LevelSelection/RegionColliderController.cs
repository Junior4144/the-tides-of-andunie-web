using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class RegionColliderController : MonoBehaviour
{
    [SerializeField] private List<GameObject> list = new();

    private void OnEnable()
    {
        CameraZoomController.OnMaxZoom += HandleMaxZoom;
        CameraZoomController.NoLongerMaxZoom += HandleNoLongerMaxZoom;
    }
    private void OnDisable()
    {
        CameraZoomController.OnMaxZoom -= HandleMaxZoom;
        CameraZoomController.NoLongerMaxZoom -= HandleNoLongerMaxZoom;
    }

    private void HandleMaxZoom()
    {
        foreach (GameObject go in list)
        {
            go.SetActive(false);
        }
    }

    private void HandleNoLongerMaxZoom()
    {
        foreach (GameObject go in list)
        {
            go.SetActive(true);
        }
    }
}
