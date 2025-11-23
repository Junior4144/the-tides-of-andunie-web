using System.Reflection;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class SellSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public InventoryItem currentItem;
    public AudioClip sellSound;
    private AudioSource audioSource;

    [Header("Scale Settings")]
    public float hoverScale = 1.05f;
    public float scaleDuration = 0.15f;

    private RectTransform rect;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        rect = GetComponent<RectTransform>();
    }
    public void SellItem()
    {
        if (currentItem == null) return;

        ShopManager.Instance.TryToSell(currentItem);

        if (sellSound != null)
            audioSource.PlayOneShot(sellSound);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        rect.DOScale(hoverScale, scaleDuration).SetEase(Ease.OutQuad);

        if (currentItem == null)
            return;

        ShopListing listing = new();
        listing.inventoryItem = currentItem;

        UIEvents.OnShopListingHover?.Invoke(listing);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        rect.DOScale(1f, scaleDuration).SetEase(Ease.OutQuad);

        UIEvents.OnShopListingExit?.Invoke();
    }
}
