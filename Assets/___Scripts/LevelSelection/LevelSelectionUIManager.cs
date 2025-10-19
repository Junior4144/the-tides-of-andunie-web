using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectionUIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject PreInvasionUI;
    [SerializeField]
    private GameObject PostInvasionUI;

    private string VillageId;
    private string Location;
    private string NextScene;

    private GameObject CurrentCanvas;

    public static LevelSelectionUIManager Instance;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    //public static event Action OnPlayerLeavingLevelSelectionZone;
    private void OnEnable() => LevelSelection.PlayerActivatedMenu += HandleMenu;

    private void OnDisable() => LevelSelection.PlayerActivatedMenu -= HandleMenu;

    void Start()
    {
        PreInvasionUI.SetActive(false); 
        PostInvasionUI.SetActive(false);
    }


    private void HandleMenu(string id, string location)
    {

        VillageState currentVillageState = LSManager.Instance.GetVillageState(id);
        Debug.Log($"[LS UI MANAGER] ID: {id} and Village State = {currentVillageState}");
        if (currentVillageState == VillageState.PreInvasion)
        {
            PreInvasionUI.SetActive(!PreInvasionUI.activeSelf);
            CurrentCanvas = PreInvasionUI;
        }
        if (currentVillageState == VillageState.Invaded)
        {
            PostInvasionUI.SetActive(!PreInvasionUI.activeSelf);
            CurrentCanvas = PostInvasionUI;
        }

        VillageId = id;
        Location = location;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CurrentCanvas.SetActive(false);
        }
    }

    public void ButtonClicked() =>
        ProceedToNextStage();

    private void ProceedToNextStage()
    {
        NextScene = LSManager.Instance.DetermineNextScene(VillageId);
        Debug.Log($"[LS UI MANAGER Next scene : {NextScene}");

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
