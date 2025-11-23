using System;
using UnityEngine;


public class ToolTipsUIController : MonoBehaviour
{
    [SerializeField] GameObject toolTipsGameObject;

    private void OnEnable()
    {
        UIEvents.OnShopListingHover += HandleUIActivation;
        UIEvents.OnShopListingExit += HandleUIDeactivation;
    }
    private void OnDisable()
    {
        UIEvents.OnShopListingHover -= HandleUIActivation;
        UIEvents.OnShopListingExit -= HandleUIDeactivation;
    }

    private void HandleUIActivation(ShopListing Listing)
    {
        toolTipsGameObject.SetActive(true);
        toolTipsGameObject.GetComponent<DyanmicToolTips>().shopListing = Listing;
    }

    private void HandleUIDeactivation()
    {
        toolTipsGameObject.SetActive(false);
    }
}
