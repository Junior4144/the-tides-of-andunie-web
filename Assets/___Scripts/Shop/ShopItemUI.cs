using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

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
        nameText.text = listing.Item.ItemName;
        priceText.text = listing.price.ToString();
        icon.sprite = listing.Item.InventoryIconPrefab.GetComponentInChildren<Image>().sprite;

        buyButton.onClick.AddListener(OnBuyClicked);
    }

    void OnBuyClicked()
    {
        ShopManager.Instance.TryToBuy(listing);
    }
}
