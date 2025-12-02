using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveGameManager : MonoBehaviour
{
    public static SaveGameManager Instance;
    private string savePath;

    [Header("References")]
    [SerializeField] private LSManager lsManager;

    [HideInInspector]
    public GameSaveData data;

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

        

        string json = JsonUtility.ToJson(data, true);

        File.WriteAllText(savePath, json);

        Debug.Log("Game Saved to: " + savePath);
    }

    public void SaveCoinData()
    {
        data.coins = CurrencyManager.Instance.Coins;
    }

    public void SaveInventoryData()
    {
        data.inventoryData = InventoryManager.Instance.GetSaveData();
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
}
