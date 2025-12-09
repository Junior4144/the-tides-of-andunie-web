using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnequipSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public InventoryItem currentItem;

    [Header("Audio")]
    [SerializeField] private AudioClip unequipSound;

    [Header("Scale Settings")]
    public float hoverScale = 1.05f;
    public float scaleDuration = 0.15f;

    private RectTransform rect;
    private AudioSource _audioSource;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        _audioSource = GetComponent<AudioSource>();
    }

    public void HandlesUnequip()
    {
        if (currentItem == null) return;

        if (unequipSound != null && _audioSource != null)
        {
            _audioSource.PlayOneShot(unequipSound);
        }

        InventoryManager.Instance.UnequipItem(currentItem?.ItemId);
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
