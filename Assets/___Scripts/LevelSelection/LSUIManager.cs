using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System;

public class LSUIManager : MonoBehaviour
{
    public static LSUIManager Instance;

    private string VillageId;
    private string Location;
    private string NextScene;
    private bool isExit;

    public static event Action DeactivatePreEntryUI;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void OnEnable() => LevelSelectionController.PlayerActivatedMenu += HandleMenu;
    private void OnDisable() => LevelSelectionController.PlayerActivatedMenu -= HandleMenu;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            DeactivatePreEntryUI?.Invoke();
        }
    }

    private void HandleMenu(string id, string location, bool isExit)
    {
        this.isExit = isExit;

        if (isExit)
        {
            SetupExitUI(location);
            return;
        }

        SetupVillageUI(id, location);
    }

    private void SetupExitUI(string location)
    {
        VillageId = "EXIT";
        Location = location;
        NextScene = "LevelSelector";

        OpenExitUI();
    }

    private void SetupVillageUI(string id, string location)
    {
        VillageState state = LSManager.Instance.GetVillageState(id);
        Debug.Log($"[LS UI MANAGER] ID: {id}, Village State = {state}");

        VillageId = id;
        Location = location;
        NextScene = LSManager.Instance.DetermineNextScene(id);

        OpenUI();
    }

    private void OpenUI()
    {
        if (LSManager.Instance != null && LSManager.Instance.GetVillageState(VillageId) == VillageState.Invaded)
        {
            Debug.Log("[LS UI MANAGER] ActivateEntryUI");
            //ActivateEntryUI?.Invoke();
            UIEvents.OnRequestPreScreenToggle?.Invoke();
        }
        else
        {
            Debug.Log("[LS UI MANAGER] EnterVillageUI");
            UIEvents.OnRequestLSEnterToggle?.Invoke(isExit);
            
        }
        
    }

    private void OpenExitUI()
    {
        Debug.Log("[LS UI MANAGER] OpenExitUI EnterVillageUI");
        UIEvents.OnRequestLSEnterToggle?.Invoke(isExit);
    }

    private void PerformExitOrVillageActions()
    {
        if (isExit)
        {
            SceneSavePositionManager.Instance.ResetPlayerPosition(gameObject.scene.name);
            Debug.Log("[LS UI MANAGER] Player position reset due to exit.");
        }
    }

    public void ButtonClicked() => ProceedToNextStage();

    private void ProceedToNextStage()
    {
        SaveManager.Instance.SaveLastLocation(Location);

        Debug.Log($"[LS UI MANAGER] Proceeding: NextScene={NextScene}, Location={Location}");

        PerformExitOrVillageActions();

        AudioManager.Instance.FadeAudio();
        SaveManager.Instance.SavePlayerStats();
        PlayerManager.Instance.HandleDestroy();

        LoadNextStage();
    }

    private void LoadNextStage()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        SceneControllerManager.Instance.LoadNextStage(currentScene, NextScene);
    }
}
