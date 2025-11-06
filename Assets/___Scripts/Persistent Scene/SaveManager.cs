using UnityEngine;
using UnityEngine.SceneManagement;

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
        if (!PlayerManager.Instance)
        {
            AssignDefaultValues();
            return;
        }
            
        CurrentSave = new PlayerSaveData()
        {
            sceneName = SceneManager.GetActiveScene().name,
            health = PlayerStatsManager.Instance.MaxHealth,
            damageAmount = PlayerStatsManager.Instance.MeleeDamage,
            lastLocation = "DefaultSpawn"
        };
        Debug.Log($"[SaveManager] OnInitalStart saves player health at {CurrentSave.health}");
        Debug.Log($"[SaveManager] OnInitalStart saves player melee at {CurrentSave.damageAmount}");
    }
    private void AssignDefaultValues()
    {
        CurrentSave = new PlayerSaveData()
        {
            sceneName = SceneManager.GetActiveScene().name,
            health = 100f,
            damageAmount = 20f,
            lastLocation = "DefaultSpawn"

        };
        Debug.Log($"[SaveManager] No Player Instance -> Default values -> Health:{CurrentSave.health}, Damage:{CurrentSave.damageAmount},");
        Debug.Log($"[SaveManager] No Player Instance -> Default values -> LastLocation:{CurrentSave.lastLocation}");
    }

    public void SavePlayerStats()
    {
        CurrentSave.sceneName = SceneManager.GetActiveScene().name;
        CurrentSave.health = PlayerManager.Instance.GetCurrentHealth();
        CurrentSave.damageAmount = PlayerManager.Instance.GetDamageAmount();
        Debug.Log($"[SaveManager] Saves player health at {CurrentSave.health}");
        Debug.Log($"[SaveManager] Saves player health at {CurrentSave.lastLocation}");
    }

    public void RestorePlayerStats()
    {
        if (CurrentSave == null)
        {
            Debug.LogWarning("[SaveManager] No save data to restore!");
            return;
        }
        PlayerManager.Instance.SetHealth(CurrentSave.health);
        Debug.Log($"[SaveManager] Restored player  health at {CurrentSave.health}");
    }

    public void ResetSaveData()
    {
        AssignDefaultValues();
        HealthUIController.Instance.UpdateHealthBar(CurrentSave.health, CurrentSave.health);
        Debug.Log("[SaveManager] Save data reset to default values.");
    }
    public void SaveLastLocation(string id)
    {
        CurrentSave.lastLocation = id;
        Debug.Log($"[SaveManager] Saved lastLocation in LevelSelector = {id}");
    }

}
