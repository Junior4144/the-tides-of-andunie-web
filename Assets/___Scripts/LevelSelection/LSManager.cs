using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum VillageState
{
    PreInvasion,
    Invaded,
    Liberated_FirstTime,
    Liberated_Done
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

    [Header("Village Data")]
    [SerializeField] private List<VillageData> villages = new List<VillageData>();

    private bool invasionStarted = false;

    [Header("Region Locks")]
    public bool _orrostarLocked = false;
    public bool _hyarrostarLocked = true;
    public bool _hyarnustarLocked = true;
    public bool _andustarLocked = true;
    public bool _forostarLocked = true;

    [Header("Global Invasion Trigger")]
    public bool startGlobalInvasion = false;
    public bool HasInvasionStarted => invasionStarted;

    public event Action<string, VillageState> OnVillageStateChanged;

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
            if (village.id == "Level1") continue;
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

    public string GetLiberatedVillageAmount()
    {
        return villages.Count(village =>
            village.state == VillageState.Liberated_FirstTime ||
            village.state == VillageState.Liberated_Done
        ).ToString();
    }

    public float GetTotalPlayableVillage()
    {
        return villages.Count(village =>
            village.region != Region.None
        );
    }

    public bool IsRegionLocked(Region region)
    {
        return region switch
        {
            Region.Orrostar => _orrostarLocked,
            Region.Hyarrostar => _hyarrostarLocked,
            Region.Hyarnustar => _hyarnustarLocked,
            Region.Andustar => _andustarLocked,
            Region.Forostar => _forostarLocked,
            _ => true,
        };
    }
}
