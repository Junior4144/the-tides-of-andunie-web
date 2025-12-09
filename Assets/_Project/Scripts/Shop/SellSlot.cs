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
    private SellUIController sellUIController;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        rect = GetComponent<RectTransform>();

        sellUIController = GetComponentInParent<SellUIController>();

        if (sellUIController == null)
        {
            Debug.LogError("[SellSlot] Could not find SellUIController in parent hierarchy!");
        }
    }
    public void SellItem()
    {
        if (currentItem == null) return;

        if (sellUIController != null && sellUIController.IsConfirmationActive)
            return;

        if (sellUIController != null)
        {
            sellUIController.ShowSellConfirmation(ExecuteSell);
        }
        else
        {
            ExecuteSell();
        }
    }

    private void ExecuteSell()
    {
        if (currentItem == null) return;

        string result = ShopManager.Instance.TryToSell(currentItem);

        if (result == "Success" && sellSound != null)
        {
            audioSource.PlayOneShot(sellSound);
        }
        else if (result != "Success")
        {
            Debug.LogWarning($"[SellSlot] Sell failed: {result}");
        }
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
