using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class UIEvents
{
    //public static Action OnRequestInventoryToggle;

    public static Action OnRequestShopToggle;

    public static Action<bool> OnRequestLSEnterToggle;

    public static Action OnRequestPreScreenToggle;

    public static Action OnRequestTutorialToggle;

    public static Action OnRequestPauseToggle;

    //public static Action OnInventoryActive;
    //public static Action OnInventoryDeactivated;

    public static Action OnRewardActive;
    public static Action OnRewardDeactivated;

    public static Action OnPauseMenuActive;
    public static Action OnPauseMenuDeactivated;

    public static Action OnShopConfirm;
    public static Action OnShopDeactivated;

    public static Action<bool> OnLSEnterConfirm;
    public static Action OnLSEnterDeactivated;

    public static Action OnPreScreenConfirm;
    public static Action OnPreScreenDeactivated;

    public static Action OnRequestCloseAllUI;

    public static Action OnTutorialActive;
    public static Action OnTutorialDeactivated;
}

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("UI Main Groups")]
    [SerializeField] private GameObject _UIPrefab;

    [Header("HUD Groups")]
    [SerializeField] private GameObject _healthBarHUD;
    [SerializeField] private GameObject _coinHUD;
    [SerializeField] private GameObject _CombatHUD;
    [SerializeField] private GameObject _PerkHUD;

    [Header("UI Groups")]
    //[SerializeField] private GameObject _inventoryUI;
    [SerializeField] private GameObject _pauseUI;

    private bool _isPaused;
    private bool _inventoryOpen;
    private bool _shopOpen;
    private bool _lSEnterUIOpen;
    private bool _preScreenUIOpen;
    private bool _tutorialOpen;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void OnEnable()
    {
        GameManager.OnGameStateChanged += HandleGameStateChanged;

        //UIEvents.OnRequestInventoryToggle += ToggleInventory;

        UIEvents.OnRequestShopToggle += ToggleShop;

        UIEvents.OnRequestPauseToggle += TogglePause;

        UIEvents.OnRequestPreScreenToggle += TogglePreScreenUI;

        UIEvents.OnRequestLSEnterToggle += ToggleLSEnterUI;

        UIEvents.OnRequestTutorialToggle += ToggleTutorialUI;

        //UIEvents.OnInventoryActive += () => _inventoryOpen = true;
        //UIEvents.OnInventoryDeactivated += () => _inventoryOpen = false;

        UIEvents.OnShopConfirm += () => _shopOpen = true;
        UIEvents.OnShopDeactivated += () => _shopOpen = false;

        UIEvents.OnLSEnterConfirm += isExit => _lSEnterUIOpen = true;
        UIEvents.OnLSEnterDeactivated += () => _lSEnterUIOpen = false;

        UIEvents.OnPreScreenConfirm += () => _preScreenUIOpen = true;
        UIEvents.OnPreScreenDeactivated += () => _preScreenUIOpen = false;

        UIEvents.OnTutorialActive += () => _tutorialOpen = true;
        UIEvents.OnTutorialDeactivated += () => _tutorialOpen = false;

        UIEvents.OnRequestCloseAllUI += CloseAllUI;
    }

    private IEnumerator Start()
    {
        yield return null;
        HandleGameStateChanged(GameManager.Instance.CurrentState);
    }
        

    private void HandleGameStateChanged(GameState newState)
    {
        //HideAll();
        Debug.Log($"UIManager responding to new state: {newState}");

        switch (newState)
        {
            case GameState.Gameplay:
                ShowGameplayUI();
                break;
            case GameState.PeacefulGameplay:
                PeacefulGameplay();
                break;
            case GameState.Stage1Gameplay:
                Stage1UI();
                break;
            case GameState.Menu:
                ShowMainMenuUI();
                break;
            case GameState.Paused:
            case GameState.Cutscene:
                ShowCutsceneUI();
                break;
            case GameState.LevelSelector:
                ShowLevelSelectorUI();
                break;
        }
    }

    private void ShowGameplayUI()
    {
        _UIPrefab.SetActive(true);
        _healthBarHUD.SetActive(true);
        _coinHUD.SetActive(!IsLevel0Stage1);
        _CombatHUD.SetActive(!IsLevel0Stage1);
        _PerkHUD.SetActive(true);
    }

    private void Stage1UI()
    {
        _UIPrefab.SetActive(false);
        _healthBarHUD.SetActive(true);
        _coinHUD.SetActive(false);
        _CombatHUD.SetActive(false);
        _PerkHUD.SetActive(false);
    }

    private void PeacefulGameplay()
    {
        _UIPrefab.SetActive(true);
        _healthBarHUD.SetActive(true);
        _coinHUD.SetActive(true);
        _CombatHUD.SetActive(false);
        _PerkHUD.SetActive(true);
    }

    private void ShowMainMenuUI()
    {
        _UIPrefab.SetActive(true);
        _healthBarHUD.SetActive(false);
        _coinHUD.SetActive(false);
        _CombatHUD.SetActive(false);
        _PerkHUD.SetActive(true);
    }

    private void ShowCutsceneUI()
    {
        _healthBarHUD.SetActive(false);
        _coinHUD.SetActive(false);
        _CombatHUD.SetActive(false);
        _UIPrefab.SetActive(false);
        _PerkHUD.SetActive(false);
    }

    private void ShowLevelSelectorUI()
    {
        _UIPrefab.SetActive(true);
        _healthBarHUD.SetActive(false);
        _coinHUD.SetActive(true);
        _CombatHUD.SetActive(false);
        _PerkHUD.SetActive(true);
    }

    private bool IsLevel0Stage1 => SceneManager.GetActiveScene().name == "Level0Stage1";

    //private void HideAll()
    //{
    //    _inventoryUI.SetActive(false);
    //}

    //private void ToggleInventory()
    //{
    //    if (_isPaused)
    //    {
    //        _inventoryUI.SetActive(false);
    //        UIEvents.OnInventoryDeactivated?.Invoke();
    //        return;
    //    }

    //    if (_inventoryUI.activeInHierarchy)
    //    {
    //        _inventoryUI.SetActive(false);
    //        UIEvents.OnInventoryDeactivated?.Invoke();
    //        return;
    //    }

    //    UIEvents.OnRequestCloseAllUI?.Invoke();

    //    _inventoryUI.SetActive(true);
    //    UIEvents.OnInventoryActive?.Invoke();
    //}

    private void ToggleShop()
    {
        if (_isPaused) // already paused
        {
            UIEvents.OnShopDeactivated?.Invoke(); // consider removing
            return;
        }

        bool isShopOpen = ShopUIController.Instance.IsOpen;

        if (isShopOpen)
        {
            UIEvents.OnShopDeactivated?.Invoke();
            return;
        }

        UIEvents.OnRequestCloseAllUI?.Invoke();
        UIEvents.OnShopConfirm?.Invoke();
    }

    private void ToggleLSEnterUI(bool isExit)
    {
        if (_isPaused)
        {
            return;
        }

        if (!_lSEnterUIOpen)
        {
            UIEvents.OnRequestCloseAllUI?.Invoke();
        }

        
        UIEvents.OnLSEnterConfirm?.Invoke(isExit);
    }


    private void TogglePreScreenUI()
    {
        if (_isPaused)
        {
            return;
        }

        if (!_preScreenUIOpen)
        {
            UIEvents.OnRequestCloseAllUI?.Invoke();
        }

        UIEvents.OnPreScreenConfirm?.Invoke();
    }

    private void ToggleTutorialUI()
    {
        if (_tutorialOpen)
        {
            return;
        }

        UIEvents.OnRequestCloseAllUI?.Invoke();
        UIEvents.OnTutorialActive?.Invoke();
    }

    private void CloseAllUI()
    {
        //if (_inventoryOpen)
        //{
        //    _inventoryUI.SetActive(false);
        //    UIEvents.OnInventoryDeactivated?.Invoke();
        //}

        if (_shopOpen)
        {
            UIEvents.OnShopDeactivated?.Invoke();
        }

        if (_preScreenUIOpen)
        {
            UIEvents.OnPreScreenDeactivated?.Invoke();
        }

        if (_lSEnterUIOpen)
        {
            UIEvents.OnLSEnterDeactivated?.Invoke();
        }

        if (_tutorialOpen)
        {
            UIEvents.OnTutorialDeactivated?.Invoke();
        }
    }

    private void TogglePause()
    {
        // 1. If ANY UI popup is open, close it and STOP pause from happening
        if (_inventoryOpen || _shopOpen || _preScreenUIOpen || _lSEnterUIOpen || _tutorialOpen)
        {
            UIEvents.OnRequestCloseAllUI?.Invoke();
            return;
        }

        // 2. If no popups are open, proceed with pause logic
        if (_isPaused)
        {
            UIEvents.OnPauseMenuDeactivated?.Invoke();
            Resume();
        }
        else
        {
            UIEvents.OnPauseMenuActive?.Invoke();
            Pause();
        }
    }

    private void Pause()
    {
        _pauseUI.SetActive(true);
        Time.timeScale = 0f;
        _isPaused = true;
    }

    private void Resume()
    {
        _pauseUI.SetActive(false);
        Time.timeScale = 1f;
        _isPaused = false;
    }
}
