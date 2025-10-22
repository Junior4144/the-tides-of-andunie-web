using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public static class UIEvents
{
    public static Action OnRequestInventoryToggle;
    public static Action OnRequestShopToggle;
}

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("UI Main Groups")]
    [SerializeField] private GameObject _InventoryUI_MainPrehab;

    [Header("UI Groups")]
    [SerializeField] private GameObject _HealthBarUI;
    [SerializeField] private GameObject _InventoryUI;

    private GameObject _ShopUI;
    private GameObject _ShopMain_UIPrehab;

    private void Awake()
    {
        if (Instance != null) return;

        Instance = this;
    }
    

    private void OnEnable()
    {
        GameManager.OnGameStateChanged += HandleGameStateChanged;
        UIEvents.OnRequestInventoryToggle += ToggleInventory;
        UIEvents.OnRequestShopToggle += ToggleShop;

    }

    private void Start()
    {
        _ShopUI = ShopUIController.Instance.canvas;
        _ShopMain_UIPrehab = ShopUIController.Instance.gameObject;
        Debug.Log($"UI MANAGER: {_ShopUI}");
        HandleGameStateChanged(GameManager.Instance.CurrentState);
    }
        

    private void HandleGameStateChanged(GameState newState)
    {
        HideAll();
        Debug.Log($"UIManager responding to new state: {newState}");

        switch (newState)
        {
            case GameState.Gameplay:
                _InventoryUI_MainPrehab.SetActive(true);
                _HealthBarUI.SetActive(true);

                if (_ShopMain_UIPrehab) _ShopMain_UIPrehab.SetActive(true);
                break;

            case GameState.Menu:
            case GameState.Paused:
            case GameState.Cutscene:
                _HealthBarUI.SetActive(false);
                _InventoryUI_MainPrehab.SetActive(false);

                if (_ShopMain_UIPrehab) _ShopMain_UIPrehab.SetActive(false);
                break;
            default:
                break;
        }
    }

    private void HideAll()
    {
        Debug.Log($"UI MANAGER HIDEALL: {_ShopUI}");
            if (_ShopUI != null)
            _ShopUI.SetActive(false);

        _InventoryUI.SetActive(false);
    }

    private void ToggleInventory()
    {
        if (_InventoryUI != null && _InventoryUI.activeInHierarchy)
        {
            HideAll();
            return;
        }

        HideAll();
        _InventoryUI.SetActive(true);
    }

    private void ToggleShop()
    {

        if (_ShopUI != null && _ShopUI.activeInHierarchy)
        {
            HideAll();
            return;
        }

        HideAll();
        _ShopUI.SetActive(true);
    }

}
