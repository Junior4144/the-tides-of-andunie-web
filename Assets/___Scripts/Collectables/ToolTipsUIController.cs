using UnityEngine;

public class ToolTipsUIController : MonoBehaviour
{
    [SerializeField] GameObject toolTipsGameObject;

    private void OnEnable()
    {
        ShopItemUI.OnShopListingHover += HandleUIActivation;
        ShopItemUI.OnShopListingExit += HandleUIDeactivation;
    }
    private void OnDisable()
    {
        ShopItemUI.OnShopListingHover -= HandleUIActivation;
        ShopItemUI.OnShopListingExit -= HandleUIDeactivation;
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
