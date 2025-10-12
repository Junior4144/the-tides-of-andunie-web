using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; private set; }
    public PlayerSaveData CurrentSave { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    private void Start()
    {
        InitializeDefaultSave();
    }

    public void InitializeDefaultSave()
    {

        CurrentSave = new PlayerSaveData()
        {
            sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name,
            health = PlayerManager.Instance.GetHealth(),
            damageAmount = PlayerManager.Instance.GetDamageAmount()

        };
        Debug.Log($"[SaveManager] OnStart saves player health at {CurrentSave.health}");
    }
    //When player beats level
    public void SavePlayerStats()
    {
        CurrentSave = new PlayerSaveData()
        {
            sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name,
            health = PlayerManager.Instance.GetHealth(),
            damageAmount = PlayerManager.Instance.GetDamageAmount()

        };
        Debug.Log($"[SaveManager] Saves player health at {CurrentSave.health}");
    }

    public void RestorePlayerStats()
    {
        if (CurrentSave == null)
        {
            Debug.LogWarning("[SaveManager] No save data to restore!");
            return;
        }
        PlayerManager.Instance.SetHealth(CurrentSave.health);
        PlayerManager.Instance.SetDamageAmount(CurrentSave.damageAmount);
        Debug.Log($"[SaveManager] Restored player  health at {CurrentSave.health}");

       
    }

}
