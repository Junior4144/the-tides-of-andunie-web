using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System;

public class LSUIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject LevelSelectionEnterUI;

    [SerializeField] private TMP_Text LevelSelectionEnterHeader;
    [SerializeField] private TMP_Text LSButtonText;

    private string VillageId;
    private string Location;
    private string NextScene;
    private bool isExit;

    private GameObject CurrentCanvas;

    public static LSUIManager Instance;

    public static event Action ActivateEntryUI;
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

    private void OnEnable() => LevelSelection.PlayerActivatedMenu += HandleMenu;
    private void OnDisable() => LevelSelection.PlayerActivatedMenu -= HandleMenu;

    private void Start() => LevelSelectionEnterUI.SetActive(false);

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
        OpenUI();

        LevelSelectionEnterHeader.text = "Leave Village";
        LSButtonText.text = "Leave";

        VillageId = "EXIT";
        Location = location;
        NextScene = "LevelSelector";
    }

    private void SetupVillageUI(string id, string location)
    {
        OpenUI();

        VillageState state = LSManager.Instance.GetVillageState(id);
        Debug.Log($"[LS UI MANAGER] ID: {id}, Village State = {state}");

        LevelSelectionEnterHeader.text = GetHeaderForState(state, id);

        VillageId = id;
        Location = location;
        NextScene = LSManager.Instance.DetermineNextScene(id);
    }

    private string GetHeaderForState(VillageState state, string id)
    {
        switch (state)
        {
            case VillageState.PreInvasion:
                return "Visit Village";
            case VillageState.Invaded:
                return "Liberate Village";
            case VillageState.Liberated_FirstTime:
            case VillageState.Liberated_Done:
                return "Visit Village";
        }
        Debug.LogError("State doesn't exist");
        return null;
    }

    private void OpenUI()
    {
        ActivateEntryUI?.Invoke();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            DeactivatePreEntryUI?.Invoke();
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

    private void PerformExitOrVillageActions()
    {
        if (isExit)
        {
            SceneSavePositionManager.Instance.ResetPlayerPosition(gameObject.scene.name);
            Debug.Log("[LS UI MANAGER] Player position reset due to exit.");
        }
    }

    private void LoadNextStage()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        SceneControllerManager.Instance.LoadNextStage(currentScene, NextScene);
    }
}
