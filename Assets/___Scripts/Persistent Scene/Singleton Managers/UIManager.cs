using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class UIEvents
{
    public static Action OnRequestInventoryToggle;

    public static Action OnRequestShopToggle;

    public static Action OnRequestPauseToggle;

    public static Action OnInventoryActive;
    public static Action OnInventoryDeactivated;

    public static Action OnRewardActive;
    public static Action OnRewardDeactivated;

    public static Action OnPauseMenuActive;
    public static Action OnPauseMenuDeactivated;

    public static Action OnShopConfirm;
    public static Action OnShopDeactivated;

    public static Action OnRequestCloseAllUI;


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
    [SerializeField] private GameObject _inventoryUI;
    [SerializeField] private GameObject _pauseUI;

    private bool _isPaused;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void OnEnable()
    {
        GameManager.OnGameStateChanged += HandleGameStateChanged;

        UIEvents.OnRequestInventoryToggle += ToggleInventory;

        UIEvents.OnRequestShopToggle += ToggleShop;

        UIEvents.OnRequestPauseToggle += TogglePause;

    }

    private IEnumerator Start()
    {
        yield return null;
        HandleGameStateChanged(GameManager.Instance.CurrentState);
    }
        

    private void HandleGameStateChanged(GameState newState)
    {
        HideAll();
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

    private void HideAll()
    {
        _inventoryUI.SetActive(false);
    }

    private void ToggleInventory()
    {
        if (GameManager.Instance.CurrentState == GameState.Paused)
        {
            _inventoryUI.SetActive(false);
            UIEvents.OnInventoryDeactivated?.Invoke();
            return;
        }

        if (_inventoryUI.activeInHierarchy)
        {
            _inventoryUI.SetActive(false);
            UIEvents.OnInventoryDeactivated?.Invoke();
            return;
        }

        UIEvents.OnRequestCloseAllUI?.Invoke();   // 🔥 close other windows first

        _inventoryUI.SetActive(true);
        UIEvents.OnInventoryActive?.Invoke();
    }

    private void ToggleShop()
    {
        if (GameManager.Instance.CurrentState == GameState.Paused)
        {
            UIEvents.OnShopDeactivated?.Invoke();
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

    GameState lastGameState;
    private void TogglePause()
    {
        if (_isPaused)
        {
            UIEvents.OnPauseMenuDeactivated?.Invoke();
            GameManager.Instance.SetState(lastGameState);
            Resume();
        }
        else
        {
            UIEvents.OnRequestCloseAllUI?.Invoke();
            UIEvents.OnPauseMenuActive?.Invoke();
            lastGameState = GameManager.Instance.CurrentState;
            GameManager.Instance.SetState(GameState.Paused);
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
