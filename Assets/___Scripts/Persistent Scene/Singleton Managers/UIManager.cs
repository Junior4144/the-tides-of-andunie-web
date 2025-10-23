using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class UIEvents
{
    public static Action OnRequestInventoryToggle;
    public static Action OnRequestShopToggle;
    public static Action OnRequestPauseToggle;
}

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("UI Main Groups")]
    [SerializeField] private GameObject _InventoryUI_MainPrehab;

    [Header("HUD Groups")]
    [SerializeField] private GameObject _HealthBarHUD;
    [SerializeField] private GameObject _CoinHUD;

    [Header("UI Groups")]
    [SerializeField] private GameObject _InventoryUI;

    private GameObject _ShopUI;
    private GameObject _ShopMain_UIPrehab;

    [SerializeField] private GameObject _PauseUI;
    private bool _isPaused;

    private void Awake()
    {
        if (Instance != null) return;

        Instance = this;
    }
    
    private void OnEnable()
    {
        SceneManager.activeSceneChanged += OnSceneChanged;
        GameManager.OnGameStateChanged += HandleGameStateChanged;
        UIEvents.OnRequestInventoryToggle += ToggleInventory;
        UIEvents.OnRequestShopToggle += ToggleShop;
        UIEvents.OnRequestPauseToggle += TogglePause;
    }

    private IEnumerator Start()
    {
        yield return null;
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
                _HealthBarHUD.SetActive(true);
                _CoinHUD.SetActive(true);

                if (SceneManager.GetActiveScene().name == "Level0Stage1")
                {
                    Debug.Log("Disabling Coin HUD");
                    _CoinHUD.SetActive(false);
                }

                if (_ShopMain_UIPrehab) _ShopMain_UIPrehab.SetActive(true);
                break;

            case GameState.Menu:
                _HealthBarHUD.SetActive(false);
                _CoinHUD.SetActive(false);

                _InventoryUI_MainPrehab.SetActive(false);

                if (_ShopMain_UIPrehab) _ShopMain_UIPrehab.SetActive(false);
                break;
            case GameState.Paused:

            case GameState.Cutscene:
                _HealthBarHUD.SetActive(false);
                _CoinHUD.SetActive(false);
                _InventoryUI_MainPrehab.SetActive(false);

                if (_ShopMain_UIPrehab) _ShopMain_UIPrehab.SetActive(false);
                break;

            default:
                break;
        }
    }

    private void HideAll()
    {
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

        if (!TryResolveShop())
            return;

        if (_ShopUI != null && _ShopUI.activeInHierarchy)
        {
            HideAll();
            return;
        }

        HideAll();
        _ShopUI.SetActive(true);
    }

    private bool TryResolveShop()
    {
        if (_ShopUI != null) return true;

        if (ShopUIController.Instance == null)
            return false;

        _ShopUI = ShopUIController.Instance.canvas;
        _ShopMain_UIPrehab = ShopUIController.Instance.gameObject;
        return true;
    }

    private void OnSceneChanged(Scene scene, Scene x)
    {
        _ShopUI = null;
        _ShopMain_UIPrehab = null;
    }

    private void TogglePause()
    {
        if (_isPaused)
        {
            _PauseUI.SetActive(false);
            Time.timeScale = 1f;
            _isPaused = false;
            return;
        }

        _PauseUI.SetActive(true);
        Time.timeScale = 0f;
        _isPaused = true;
    }
}
