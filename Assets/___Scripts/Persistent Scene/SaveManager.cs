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
            health = PlayerManager.Instance.GetHealth(),
            damageAmount = PlayerManager.Instance.GetDamageAmount()

        };
        Debug.Log($"[SaveManager] OnStart saves player health at {CurrentSave.health}");
    }
    private void AssignDefaultValues()
    {
        CurrentSave = new PlayerSaveData()
        {
            sceneName = SceneManager.GetActiveScene().name,
            health = 100f,
            damageAmount = 20f

        };
        Debug.Log($"[SaveManager] No Player Instance -> Default values -> Health:{CurrentSave.health}, Damage:{CurrentSave.damageAmount},");
    }

    public void SavePlayerStats()
    {
        CurrentSave = new PlayerSaveData()
        {
            sceneName = SceneManager.GetActiveScene().name,
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

    public void ResetSaveData()
    {
        AssignDefaultValues();
        UIManager.Instance.UpdateHealtBar(CurrentSave.health, CurrentSave.health);
        Debug.Log("[SaveManager] Save data reset to default values.");
    }

}
