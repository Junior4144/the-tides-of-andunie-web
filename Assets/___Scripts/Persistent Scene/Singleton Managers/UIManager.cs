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


    [Header("UI Groups")]
    [SerializeField] private GameObject _inventoryUI;

    private GameObject _shopUI;
    private GameObject _shopUIPrefab;

    [SerializeField] private GameObject _pauseUI;
    private bool _isPaused;

    private void Awake()
    {
        if (Instance == null)
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
            case GameState.Menu:
                ShowMenuUI();
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

        if (_shopUIPrefab)
            _shopUIPrefab.SetActive(true);
    }

    private void ShowMenuUI()
    {
        _healthBarHUD.SetActive(false);
        _coinHUD.SetActive(false);
        _CombatHUD.SetActive(false);
        _UIPrefab.SetActive(false);

        if (_shopUIPrefab)
            _shopUIPrefab.SetActive(false);
    }

    private void ShowCutsceneUI()
    {
        _healthBarHUD.SetActive(false);
        _coinHUD.SetActive(false);
        _CombatHUD.SetActive(false);
        _UIPrefab.SetActive(false);

        if (_shopUIPrefab)
            _shopUIPrefab.SetActive(false);
    }

    private void ShowLevelSelectorUI()
    {
        _UIPrefab.SetActive(true);
        _healthBarHUD.SetActive(false);
        _coinHUD.SetActive(true);
        _CombatHUD.SetActive(false);

        if (_shopUIPrefab)
            _shopUIPrefab.SetActive(false);
    }

    private bool IsLevel0Stage1 => SceneManager.GetActiveScene().name == "Level0Stage1";

    private void HideAll()
    {
        if (_shopUI != null)
            _shopUI.SetActive(false);

        _inventoryUI.SetActive(false);
    }

    private void ToggleInventory()
    {
        if (_inventoryUI.activeInHierarchy)
        {
            _inventoryUI.SetActive(false);
            UIEvents.OnInventoryDeactivated.Invoke();
            return;
        }

        HideAll();
        _inventoryUI.SetActive(true);
        UIEvents.OnInventoryActive.Invoke();
    }

    private void ToggleShop()
    {
        if (!TryResolveShop())
            return;

        if (_shopUI.activeInHierarchy)
        {
            _shopUI.SetActive(false);
            return;
        }

        HideAll();
        _shopUI.SetActive(true);
    }

    private bool TryResolveShop()
    {
        if (_shopUI != null) return true;

        if (ShopUIController.Instance == null)
            return false;

        _shopUI = ShopUIController.Instance.canvas;
        _shopUIPrefab = ShopUIController.Instance.gameObject;
        return true;
    }

    private void OnSceneChanged(Scene oldScene, Scene newScene)
    {
        _shopUI = null;
        _shopUIPrefab = null;
    }

    private void TogglePause()
    {
        if (_isPaused)
            Resume();
        else
            Pause();
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
