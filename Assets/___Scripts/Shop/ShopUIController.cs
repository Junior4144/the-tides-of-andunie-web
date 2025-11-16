using System;
using UnityEngine;

public class ShopUIController : MonoBehaviour
{
    public static ShopUIController Instance { get; private set; } // TODO consider changing this to be named manager


    [SerializeField] private GameObject mainShopPanel;
    [SerializeField] private GameObject shopItemContainer;
    [SerializeField] private GameObject canvas;

    [SerializeField] private GameObject shopItemUIPrefab;

    public bool IsOpen => mainShopPanel.activeInHierarchy;

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
        UIEvents.OnShopConfirm += HandleShopToggling;
        UIEvents.OnShopDeactivated += HandleShopDeactivation;
        UIEvents.OnRequestCloseAllUI += HandleShopDeactivation;
    }

    private void OnDisable()
    {
        UIEvents.OnShopConfirm -= HandleShopToggling;
        UIEvents.OnShopDeactivated -= HandleShopDeactivation;
        UIEvents.OnRequestCloseAllUI -= HandleShopDeactivation;
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
        HandleShopDeactivation();
    }

    public void HandleShopToggling()
    {
        if (mainShopPanel.activeInHierarchy)
        {
            HandleShopDeactivation();
        }
            
        else
        {
            ShopActivated?.Invoke();
            mainShopPanel.SetActive(true);
        }
            
    }

    public void HandleShopDeactivation()
    {
        ShopDeactivated?.Invoke();
        mainShopPanel.SetActive(false);
    }
}