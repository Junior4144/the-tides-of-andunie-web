using UnityEngine;

public class ShopUIController : MonoBehaviour
{
    [SerializeField] private Transform shopPanel;
    [SerializeField] private GameObject shopItemUIPrefab;
    [SerializeField] private GameObject canvas;

    void Start()
    {
        PopulateShop();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            canvas.SetActive(!canvas.activeInHierarchy);
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