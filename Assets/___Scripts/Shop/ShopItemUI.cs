using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopItemUI : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text priceText;
    [SerializeField] private Button buyButton;


    private ShopListing listing;

    public void SetData(ShopListing listing)
    {
        this.listing = listing;
        nameText.text = listing.inventoryItem.ItemName;
        priceText.text = listing.price.ToString();
        icon.sprite = listing.inventoryItem.InventoryIconPrefab.GetComponent<Image>().sprite;

        buyButton.onClick.AddListener(OnBuyClicked);
    }

    void OnBuyClicked()
    {
        ShopManager.Instance.TryToBuy(listing);
    }
}
