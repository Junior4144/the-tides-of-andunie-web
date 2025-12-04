using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public InventoryItem currentItem;

    [Header("Audio")]
    [SerializeField] private AudioClip equipSound;

    [Header("Scale Settings")]
    public float hoverScale = 1.05f;
    public float scaleDuration = 0.15f;

    private RectTransform rect;
    private AudioSource _audioSource;

    private bool _disableHover = false;

    private void OnEnable()
    {
        UIEvents.OnShopDeactivated += HandleChange;


        UIEvents.OnPreScreenDeactivated += HandleChange;
        UIEvents.OnPauseMenuActive += HandleChange;

        UIEvents.OnPauseMenuDeactivated += RevertChanges;
    }

    private void OnDisable()
    {
        UIEvents.OnShopDeactivated -= HandleChange;

        UIEvents.OnPreScreenDeactivated -= HandleChange;
        UIEvents.OnPauseMenuActive -= HandleChange;

        UIEvents.OnPauseMenuDeactivated -= RevertChanges;
    }

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        _audioSource = GetComponent<AudioSource>();
    }

    private void HandleChange()
    {
        _disableHover = true;
    }

    private void RevertChanges()
    {
        _disableHover = false;
    }

    public void EquipItem()
    {
        if (currentItem == null) return;

        if (equipSound != null && _audioSource != null)
        {
            _audioSource.PlayOneShot(equipSound);
        }

        InventoryManager.Instance.EquipAllOfItem(currentItem.ItemId);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_disableHover) return;

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
    