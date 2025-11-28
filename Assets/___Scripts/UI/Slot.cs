using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public InventoryItem currentItem;

    [Header("Scale Settings")]
    public float hoverScale = 1.05f;
    public float scaleDuration = 0.15f;

    private RectTransform rect;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    public void EquipItem()
    {
        if (currentItem == null) return;
        InventoryManager.Instance.EquipAllOfItem(currentItem.ItemId);
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
    