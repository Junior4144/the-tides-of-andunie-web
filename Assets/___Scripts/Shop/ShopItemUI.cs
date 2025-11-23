using System;
using System.Collections;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopItemUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text priceText;
    [SerializeField] private Button buyButton;
    [SerializeField] private TMP_Text ErrorText;

    [SerializeField] private AudioClip purchaseSound;
    [SerializeField] private AudioClip purchaseErrorSound;

    [Header("Scale Settings")]
    public float hoverScale = 1.05f;
    public float scaleDuration = 0.15f;

    private AudioSource audioSource;
    private ShopListing listing;
    private RectTransform rect;


    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        rect = GetComponent<RectTransform>();
    }

    private void Start() => ErrorText.gameObject.SetActive(false);

    private void OnDisable() => ErrorText.gameObject.SetActive(false);

    public void SetData(ShopListing listing)
    {
        this.listing = listing;
        nameText.text = listing.Item.ItemName.Replace(" ", "  ");
        priceText.text = $"Price: {listing.price}";
        icon.sprite = listing.Item.SpriteIcon;

        buyButton.onClick.AddListener(OnBuyClicked);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        rect.DOScale(hoverScale, scaleDuration).SetEase(Ease.OutQuad);
        UIEvents.OnShopListingHover?.Invoke(listing);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        rect.DOScale(1f, scaleDuration).SetEase(Ease.OutQuad);
        UIEvents.OnShopListingExit?.Invoke();
    }

    void OnBuyClicked()
    {
        string error = ShopManager.Instance.TryToBuy(listing);

        if (error == "Success") HandleSuccessfulPurchase();
        if (error == "NotEnough") HandleNotEnoughCoins();
        if (error == "LimitReached") HandleLimitReached();
    }

    private void HandleSuccessfulPurchase()
    {
        audioSource.PlayOneShot(purchaseSound);
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
        audioSource.PlayOneShot(purchaseErrorSound);
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

    public void ResetErrors()
    {
        ErrorText.gameObject.SetActive(false);
    }

}
