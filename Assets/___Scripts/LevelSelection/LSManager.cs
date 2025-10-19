using System;
using System.Collections.Generic;
using UnityEngine;

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
    public string id;          // "Village1", "Village2", etc.
    public VillageState state;
}

public class LSManager : MonoBehaviour
{
    public static LSManager Instance;

    [SerializeField] private List<VillageData> villages = new List<VillageData>();

    public bool invasionStarted = false;

    // GLOBAL EVENTS
    public event Action OnGlobalInvasionStarted;

    // PER-VILLAGE EVENT
    public event Action<string, VillageState> OnVillageStateChanged;

    void Awake()
    {
        // --- persistent singleton boilerplate ---
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }


    // =============================
    // READ METHODS
    // =============================

    public VillageState GetVillageState(string villageId)
    {
        var v = villages.Find(v => v.id == villageId);
        if (v == null)
        {
            Debug.LogError($"Village ID not found: {villageId}");
            return VillageState.PreInvasion; // fallback
        }
        return v.state;
    }

    public bool IsVillageLiberated(string villageId)
    {
        var state = GetVillageState(villageId);
        return state == VillageState.Liberated_FirstTime
            || state == VillageState.Liberated_Done;
    }


    // =============================
    // WRITE METHODS
    // =============================

    public void StartGlobalInvasion()
    {
        if (invasionStarted) return;

        invasionStarted = true;
        OnGlobalInvasionStarted?.Invoke();
    }

    public void SetVillageState(string villageId, VillageState newState)
    {
        var village = villages.Find(v => v.id == villageId);
        if (village == null)
        {
            Debug.LogError($"Village ID not found: {villageId}");
            return;
        }

        if (village.state == newState)
            return; // no change

        village.state = newState;
        OnVillageStateChanged?.Invoke(villageId, newState);
    }
}
