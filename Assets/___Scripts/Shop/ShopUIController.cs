using System;
using UnityEngine;

public class ShopUIController : MonoBehaviour
{
    public static ShopUIController Instance { get; private set; } // TODO consider changing this to be named manager


    [SerializeField] private GameObject mainShopPanel;
    [SerializeField] private GameObject shopItemContainer;
    [SerializeField] private GameObject canvas;

    [SerializeField] private GameObject shopItemUIPrefab;

    public static event Action ShopActivated;
    public static event Action ShopDeactivated;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void OnEnable()
    {
        UIEvents.OnShopConfirm += HandleShopUIActivation;
        UIEvents.OnShopDeactivated += HandleShopDeactivation;
    }

    private void OnDisable()
    {
        UIEvents.OnShopConfirm -= HandleShopUIActivation;
        UIEvents.OnShopDeactivated -= HandleShopDeactivation;
    }

    void Start()
    {
        PopulateShop();
    }

    void PopulateShop()
    {
        ShopListing[] listings = ShopManager.Instance.GetListings();

        foreach (var listing in listings)
        {
            var uiObj = Instantiate(shopItemUIPrefab, shopItemContainer.transform);
            var ui = uiObj.GetComponent<ShopItemUI>();

            ui.SetData(listing);
        }
    }

    public void HandleExitClick()
    {
        UIEvents.OnShopDeactivated?.Invoke();
    }

    public void HandleShopUIActivation()
    {
        var scaler = mainShopPanel.GetComponent<ScaleOnEnable>();

        if (scaler.IsAnimating)
            return;


        ShopActivated?.Invoke();
        mainShopPanel.SetActive(true);

    }

    public void HandleShopDeactivation()
    {
        var scaler = mainShopPanel.GetComponent<ScaleOnEnable>();

        if (mainShopPanel.activeInHierarchy && !scaler.IsAnimating)
        {
            scaler.HideWithScale();
        }
        else
        {
            mainShopPanel.SetActive(false);
        }

            ShopDeactivated?.Invoke();
    }
}