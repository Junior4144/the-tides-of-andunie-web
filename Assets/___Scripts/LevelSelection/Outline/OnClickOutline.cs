using UnityEngine;
using System;

public enum Region
{
    Orrostar,
    Hyarrostar,
    Hyarnustar,
    Andustar,
    Forostar,
}

public class OnClickOutline : MonoBehaviour
{
    [SerializeField] private Region region;

    public static event Action<Region> RegionClicked;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("[OnClickOutline]Clicking down");
            RegionClicked?.Invoke(region);
        }
    }
}
