using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class RegionEventBus
{
    public static Action OnDisableOfRegionUI;
    public static Action NoLongerDisableOfRegionUI;
}

public class RegionZoomController : MonoBehaviour
{
    public float threshold = 200f;
    public float PreInvasionThreshold = 400f;

}