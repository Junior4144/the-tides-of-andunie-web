using System.IO;
using UnityEngine;

public class SaveGameManager : MonoBehaviour
{
    public static SaveGameManager Instance;

    [Header("References")]
    [SerializeField] private LSManager lsManager;

    [HideInInspector]
    public GameSaveData data;

    private string savePath = "";

    [System.Serializable]
    public class GameSaveData
    {
        public string lastScene;
        public Vector3 lastPlayerPos;
        public Quaternion lastPlayerRot;

        public LSManagerSave levelSelectorData;
        public PlayerSaveData playerSaveData;

        public int coins;

        public InventorySaveData inventoryData;

        public StorySaveData storyData;

        public RegionLockSaveData regionLockData;
    }

    [System.Serializable]
    public class StorySaveData
    {
        public string lastLiberatedVillageID;

        public bool playTownhallCutscene;
        public bool enterLevelSelectorFirstTime;
        public bool playLSInvasionCutscene;
        public bool hasTalkedToChief;
        public bool hasExitedLiberation;

        public bool showWaypoints;
        public bool comingFromPauseMenu;
    }

    [System.Serializable]
    public class RegionLockSaveData
    {
        public bool orrostarLocked;
        public bool hyarrostarLocked;
        public bool hyarnustarLocked;
        public bool andustarLocked;
        public bool forostarLocked;
    }

    void Awake()
    {
        savePath = Application.persistentDataPath + "/save.json";
        Debug.Log("Save path: " + savePath);

        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }


    public void SaveGame()
    {
        GameSaveData data = new GameSaveData();

        data.levelSelectorData = lsManager.GetSaveData();

        data.playerSaveData = SaveManager.Instance.CurrentSave;
        data.coins = CurrencyManager.Instance.Coins;

        data.lastScene = SceneSavePositionManager.Instance.LastSceneName;
        data.lastPlayerPos = SceneSavePositionManager.Instance.LastPosition;
        data.lastPlayerRot = SceneSavePositionManager.Instance.LastRotation;

        data.inventoryData = InventoryManager.Instance.GetSaveData();

        data.storyData = GlobalStoryManager.Instance.GetSaveData();
        data.regionLockData = LSRegionLockManager.Instance.GetSaveData();

        string json = JsonUtility.ToJson(data, true);

        File.WriteAllText(savePath, json);

        Debug.Log("Game Saved to: " + savePath);
    }

    public void LoadGame()
    {
        if (!File.Exists(savePath))
        {
            Debug.LogWarning("No save file found. Nothing loaded.");
            return;
        }

        string json = File.ReadAllText(savePath);

        data = JsonUtility.FromJson<GameSaveData>(json);

        lsManager.ApplySaveData(data.levelSelectorData);
        SaveManager.Instance.SetPlayerSave(data.playerSaveData);
        CurrencyManager.Instance.SetCoins(data.coins);

        InventoryManager.Instance.ApplySaveData(data.inventoryData);

        GlobalStoryManager.Instance.ApplySaveData(data.storyData);
        LSRegionLockManager.Instance.ApplySaveData(data.regionLockData);

        SceneSavePositionManager.Instance.SaveLastScene(data.lastScene);
        Debug.Log("Game Loaded");
    }


    public bool CheckSaveFile()
    {
        if (!File.Exists(savePath))
        {
            Debug.LogWarning("No save file found. Nothing loaded.");
            return false;
        }
        return true;
    }

    public void NewGame()
    {
        InventoryManager.Instance.ResetInventory();
        CurrencyManager.Instance.SetCoins(0);
        GlobalStoryManager.Instance.ResetStoryState();
        LSRegionLockManager.Instance.ResetRegionLocks();
        SaveManager.Instance.ResetToDefaults();
        SceneSavePositionManager.Instance.ResetSceneSaveData();
        LSManager.Instance.ResetLSManager();
    }
}
