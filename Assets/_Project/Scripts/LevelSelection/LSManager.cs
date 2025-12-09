using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

[Serializable]
public class VillageSaveData
{
    public string id;
    public VillageState state;
}

[Serializable]
public class LSManagerSave
{
    public List<VillageSaveData> villages = new();
    public bool invasionStarted;
}

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
public class TargetEvents
{
    public static Action OnClearAllTargets;
}

public class LSManager : MonoBehaviour
{
    public static LSManager Instance;

    [Header("Village Data")]
    [SerializeField] private List<VillageData> villages = new List<VillageData>();


    private Dictionary<string, VillageState> defaultStates;

    private bool invasionStarted = false;

    [Header("Global Invasion Trigger")]
    public bool startGlobalInvasion = false;
    public bool HasInvasionStarted => invasionStarted;

    public event Action<string, VillageState> OnVillageStateChanged;

    public static event Action GlobalInvasionTriggered;


    void Awake()
    {
        if (Instance != null && Instance != this) {Destroy(gameObject); return;}
        Instance = this;
        Debug.Log($"LSManager Awake — villages count: {villages.Count}", this);
        
        defaultStates = new Dictionary<string, VillageState>();

        foreach (var v in villages)
        {
            defaultStates[v.id] = v.state;
        }
    }

    private void OnEnable()
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
        GlobalStoryManager.Instance.LastLiberatedVillageID = villageId;
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

    public string GetVillageName(string villageId)
    {
        var village = villages.Find(village => village.id == villageId);
        Debug.Log($"LSManager {villages}");

        if (village == null) return "Village Not Found";
        return village.villageName;
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

        GlobalInvasionTriggered?.Invoke();
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
            village.id != "Level1" &&
            (village.state == VillageState.Liberated_FirstTime ||
             village.state == VillageState.Liberated_Done)
        ).ToString();
    }

    public float GetTotalPlayableVillage()
    {
        return villages.Count(village =>
            village.id != "Level1" &&
            village.region != Region.None
        );
    }

    public bool IsRegionFullyLiberated(Region region)
    {
        var regionVillages = villages.Where(v => v.region == region);

        // TRUE only if EVERY village is liberated
        return regionVillages.All(v =>
            v.state == VillageState.PreInvasion ||
            v.state == VillageState.Liberated_FirstTime ||
            v.state == VillageState.Liberated_Done
        );
    }

    public LSManagerSave GetSaveData()
    {
        LSManagerSave save = new LSManagerSave();

        foreach (var v in villages)
        {
            save.villages.Add(new VillageSaveData
            {
                id = v.id,
                state = v.state
            });
        }

        save.invasionStarted = invasionStarted;

        return save;
    }

    public void ApplySaveData(LSManagerSave data)
    {
        foreach (var savedVillage in data.villages)
        {
            var village = villages.Find(v => v.id == savedVillage.id);
            if (village != null)
            {
                village.state = savedVillage.state;
            }
        }
        invasionStarted = data.invasionStarted;
    }

    public void ResetLSManager()
    {
        invasionStarted = false;

        foreach (var v in villages)
        {
            if (defaultStates.TryGetValue(v.id, out var defaultState))
            {
                v.state = defaultState;
            }
            else
            {
                // fallback just in case a new village was added
                v.state = VillageState.PreInvasion;
            }
        }

        Debug.Log("[LSManager] Reset complete — default states restored");
    }
}
