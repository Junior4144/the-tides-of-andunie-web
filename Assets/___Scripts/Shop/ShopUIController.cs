using UnityEngine;

public class ShopUIController : MonoBehaviour
{
    public static ShopUIController Instance { get; private set; }

    [SerializeField] private Transform shopPanel;
    [SerializeField] private GameObject shopItemUIPrefab;
    [SerializeField] public GameObject canvas;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    void Start()
    {
        PopulateShop();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G)) // only for debugging
        {
            UIEvents.OnRequestShopToggle?.Invoke();

        }
    }
    void PopulateShop()
    {
        ShopListing[] listings = ShopManager.Instance.GetListings();

        foreach (var listing in listings)
        {
            var uiObj = Instantiate(shopItemUIPrefab, shopPanel);
            var ui = uiObj.GetComponent<ShopItemUI>();

            ui.SetData(listing);
        }
    }
}