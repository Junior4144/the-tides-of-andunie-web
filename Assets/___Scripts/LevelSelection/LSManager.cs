using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum VillageState
{
    PreInvasion,
    Invaded,
    Liberated_FirstTime,   // cutscene not yet played
    Liberated_Done         // cutscene already played
}

[Serializable]
public class VillageData
{
    public string id;
    public string villageName;
    public string SceneName;
    public int diffculty;
    public VillageState state = VillageState.PreInvasion;
    public Region region;
    public RewardsConfig rewardConfig;
    
}

public class LSManager : MonoBehaviour
{
    public static LSManager Instance;

    [SerializeField] private List<VillageData> villages = new List<VillageData>();

    private bool invasionStarted = false;
    public bool startGlobalInvasion = false;

    public event Action<string, VillageState> OnVillageStateChanged;

    public bool HasInvasionStarted => invasionStarted;


    void Awake()
    {
        if (Instance != null && Instance != this) {Destroy(gameObject); return;}
        Instance = this;

    }

    private void Start()
    {
        if (startGlobalInvasion)
        {
            TriggerGlobalInvasion();
        }
        else if (SceneManager.GetActiveScene().name == "LevelSelector")
        {
            Debug.Log("[LS Manager] LSManager started inside LevelSelector — manual invoke");
        }

    }

    public void SetVillageState(string villageId, VillageState newState)
    {
        var village = villages.Find(village => villageId == village.id);

        if (village == null)
        {
            Debug.LogError($"Village ID not found: {villageId}");
            return;
        }

        village.state = newState;
        OnVillageStateChanged?.Invoke(villageId, newState);
        return;
    }

    public VillageState GetVillageState(string villageId)
    {
        var village = villages.Find(village => village.id == villageId);

        if (village == null)
        {
            Debug.LogError($"Village ID not found: {villageId}");
            return VillageState.PreInvasion;
        }

        return village.state;
    }

    public void TriggerGlobalInvasion()
    {
        if (invasionStarted) return;
        invasionStarted = true;

        Debug.Log("Global Invasion Starting");
        foreach (var village in villages)
        {
            village.state = VillageState.Invaded;
            OnVillageStateChanged?.Invoke(village.id, VillageState.Invaded);
        }
    }

    public string DetermineNextScene(string villageId)
    {
        if (villageId == "EXIT")
            return "LevelSelector";

        for (int i = 0; i < villages.Count; i++)
        {
            if (villages[i].id == villageId)
            {
                var data = villages[i];
                return data.SceneName;
            }
        }
        Debug.LogError($"Village not found: {villageId}");
        return null;
    }

    public List<VillageData> GetVillagesByRegion(Region region)
    {
        return villages
            .Where(village => village.region == region)
            .ToList();
    }
}
