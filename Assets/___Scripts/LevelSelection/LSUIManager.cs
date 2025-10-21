using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LSUIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject LevelSelectionEnterUI;

    [SerializeField] private TMP_Text LevelSelectionEnterHeader;
    [SerializeField] private TMP_Text LSButtonText;

    private string VillageId;
    private string Location;
    private string NextScene;

    private GameObject CurrentCanvas;

    public static LSUIManager Instance;


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

    void Start() => LevelSelectionEnterUI.SetActive(false);

    private void HandleMenu(string id, string location)
    {
        if (id == "EXIT")
        {
            LevelSelectionEnterUI.SetActive(true);
            CurrentCanvas = LevelSelectionEnterUI;
            LevelSelectionEnterHeader.text = "Leave Village";
            LSButtonText.text = "Leave";

            VillageId = id;
            Location = location;
            return;
        }

        VillageState currentVillageState = LSManager.Instance.GetVillageState(id);
        Debug.Log($"[LS UI MANAGER] ID: {id} and Village State = {currentVillageState}");

        LevelSelectionEnterUI.SetActive(true);
        CurrentCanvas = LevelSelectionEnterUI;

        switch (currentVillageState)
        {
            case VillageState.PreInvasion:
                LevelSelectionEnterHeader.text = "Visit Village";
                break;

            case VillageState.Invaded:
                if (id == "Village7") LevelSelectionEnterHeader.text = "Visit Village";
                else LevelSelectionEnterHeader.text = "Liberate Village";
                break;

            case VillageState.Liberated_FirstTime:
            case VillageState.Liberated_Done:
                LevelSelectionEnterHeader.text = "Visit Village";
                break;
        }

        VillageId = id;
        Location = location;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && CurrentCanvas != null)
        {
            CurrentCanvas.SetActive(false);
            CurrentCanvas = null;
        }
    }

    public void ButtonClicked() =>
        ProceedToNextStage();

    private void ProceedToNextStage()
    {
        NextScene = LSManager.Instance.DetermineNextScene(VillageId);
        Debug.Log($"[LS UI MANAGER] Next scene : {NextScene}");

        SaveManager.Instance.SaveLastLocation(Location);

        Debug.Log($"[EndCurrentScene] Next Village: {NextScene} and Location: {Location}");

        GameObject _player = PlayerManager.Instance.gameObject;
        Debug.Log($"Player: {_player.name} and saving data");

        AudioManager.Instance.FadeAudio();
        SaveManager.Instance.SavePlayerStats();
        PlayerManager.Instance.HandleDestroy();

        LoadNextStage();
    }

    private void LoadNextStage() =>
        SceneControllerManager.Instance.LoadNextStage(SceneManager.GetActiveScene().name, NextScene);
}
