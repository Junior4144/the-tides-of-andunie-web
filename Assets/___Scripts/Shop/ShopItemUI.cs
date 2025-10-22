using System.Collections;
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
    [SerializeField] private TMP_Text ErrorText;

    private ShopListing listing;
    private void Start() => ErrorText.gameObject.SetActive(false);


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
        string error = ShopManager.Instance.TryToBuy(listing);

        if (error == "NotEnough") HandleNotEnoughCoins();

        if (error == "LimitReached") HandleLimitReached();
    }

    private void HandleNotEnoughCoins()
    {
        StopAllCoroutines();
        StartCoroutine(NotEnoughCoins());
    }

    private IEnumerator NotEnoughCoins()
    {
        ErrorText.text = "Not Enough Coins";
        ErrorText.gameObject.SetActive(true);

        yield return new WaitForSeconds(1f);

        ErrorText.gameObject.SetActive(false);
    }

    private void HandleLimitReached() { StopAllCoroutines(); StartCoroutine(LimitReached()); }

    private IEnumerator LimitReached()
    {
        ErrorText.text = "Limit Reached";
        ErrorText.gameObject.SetActive(true);

        yield return new WaitForSeconds(1f);

        ErrorText.gameObject.SetActive(false);
    }
}
