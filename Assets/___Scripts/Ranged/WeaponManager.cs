using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum WeaponType
{
    none,
    Axe,
    Bow,
}

public static class WeaponEvents
{
    public static Action<WeaponType> OnEquipWeaponRequest;
    public static Action<WeaponType> OnNewWeaponEquipped;
    public static Action<WeaponType> OnWeaponAbilityActivation;
}

public class WeaponManager : MonoBehaviour
{
    public static WeaponManager Instance { get; private set; }

    [SerializeField] private WeaponType currentWeapon = WeaponType.none;

    public bool IsBusy { get; private set; } = true;

    [HideInInspector] public float CurrentBowCharge;
    [HideInInspector] public float BowMaxCharge;
    [HideInInspector] public bool IsNormalAiming;
    [HideInInspector] public bool IsAbilityAiming;

    private GameState currentGameState;
    private WeaponType? pendingWeaponRequest = null;
    private string currentSceneName;


    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void OnEnable()
    {
        WeaponEvents.OnEquipWeaponRequest += HandleEquipRequest;
        GameManager.OnGameStateChanged += HandleGameStateChanged;
        //SceneManager.activeSceneChanged += OnSceneChanged;

        //UIEvents.OnInventoryActive += HandlePopUpUIActive;
        //UIEvents.OnInventoryDeactivated += OnPopUpUIDeactivated;

        UIEvents.OnRewardActive += HandlePopUpUIActive;
        UIEvents.OnRewardDeactivated += OnPopUpUIDeactivated;

        UIEvents.OnTutorialActive += HandlePopUpUIActive;
        UIEvents.OnTutorialDeactivated += OnPopUpUIDeactivated;

        UIEvents.OnPauseMenuActive += HandlePopUpUIActive;
        UIEvents.OnPauseMenuDeactivated += OnPopUpUIDeactivated;
    }

    private void OnDisable()
    {
        WeaponEvents.OnEquipWeaponRequest -= HandleEquipRequest;
        GameManager.OnGameStateChanged -= HandleGameStateChanged;
        //SceneManager.activeSceneChanged -= OnSceneChanged;

        //UIEvents.OnInventoryActive -= HandlePopUpUIActive;
        //UIEvents.OnInventoryDeactivated -= OnPopUpUIDeactivated;

        UIEvents.OnRewardActive -= HandlePopUpUIActive;
        UIEvents.OnRewardDeactivated -= OnPopUpUIDeactivated;

        UIEvents.OnTutorialActive -= HandlePopUpUIActive;
        UIEvents.OnTutorialDeactivated -= OnPopUpUIDeactivated;

        UIEvents.OnPauseMenuActive -= HandlePopUpUIActive;
        UIEvents.OnPauseMenuDeactivated -= OnPopUpUIDeactivated;
    }

    private void Start()
    {
        HandleEquipRequest(WeaponType.Axe);
    }

    private void HandleEquipRequest(WeaponType requestedWeapon)
    {
        if (currentGameState != GameState.Gameplay)
        {
            Debug.Log("Weapon logic disabled due to game state.");
            return;
        }

        if (IsBusy || IsAbilityAiming || IsNormalAiming)
        {
            pendingWeaponRequest = requestedWeapon;
            Debug.Log($"Weapon switch to {requestedWeapon} queued (currently busy).");
            return;
        }

        EquipWeapon(requestedWeapon);
    }

    private void EquipWeapon(WeaponType newWeapon)
    {
        currentWeapon = newWeapon;
        Debug.Log($"Equipped weapon: {newWeapon}");
        WeaponEvents.OnNewWeaponEquipped?.Invoke(newWeapon);
    }

    private void HandleGameStateChanged(GameState newState)
    {
        currentGameState = newState;

        currentSceneName = SceneManager.GetActiveScene().name;


        if (newState != GameState.Gameplay)
        {
            Debug.Log($"Scene '{currentSceneName}' detected — disabling all weapons.");
            IsBusy = true;
            SetWeaponToNone();
        }
        else
        {
            Debug.Log($"Scene '{currentSceneName}' detected or GAMEPLAY — enabling weapons if gameplay.");
            IsBusy = false;
            HandleEquipRequest(WeaponType.Axe);
        }
    }
    private void SetWeaponToNone()
    {
        if (currentWeapon != WeaponType.none)
        {
            currentWeapon = WeaponType.none;
            WeaponEvents.OnNewWeaponEquipped?.Invoke(WeaponType.none);
        }
    }

    private void HandlePopUpUIActive()
    {
        WeaponEvents.OnNewWeaponEquipped?.Invoke(WeaponType.none);
        IsBusy = true;
    }
    
    private void OnPopUpUIDeactivated()
    {
        Debug.Log("OnPopUpUIDeactivated is called");
        IsBusy = false;
        HandleEquipRequest(WeaponType.Axe);
    }

    public void SetBusy(bool value)
    {
        bool wasBusy = IsBusy;
        IsBusy = value;

        if (wasBusy && !IsBusy)
        {
            if (pendingWeaponRequest.HasValue)
            {
                WeaponType queuedWeapon = pendingWeaponRequest.Value;
                pendingWeaponRequest = null;
                EquipWeapon(queuedWeapon);
            }
        }
    }

    public void DisableWeapon()
    {
        IsBusy = true;
        SetWeaponToNone();
    }

    public WeaponType GetCurrentWeapon()
    {
        return currentWeapon;
    }
}
