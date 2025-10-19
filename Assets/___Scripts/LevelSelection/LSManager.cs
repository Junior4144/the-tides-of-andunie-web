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
    public VillageState state = VillageState.PreInvasion;
    public string preSceneName;
    public string postSceneName;

}

public class LSManager : MonoBehaviour
{
    public static LSManager Instance;

    [SerializeField] private List<VillageData> villages = new List<VillageData>();

    private bool invasionStarted = false;
    public bool startGlobalInvasion = false;

    public event Action<string, VillageState> OnVillageStateChanged;
    public static event Action OnGlobalInvasionStarted; // for level 1 leave to be triggered

    public bool HasInvasionStarted => invasionStarted;


    void Awake()
    {
        if (Instance != null && Instance != this) {Destroy(gameObject); return;}
        Instance = this;

    }
    private void Start()
    {
        if (startGlobalInvasion)
            TriggerGlobalInvasion();
    }

    public void SetVillageState(string villageId, VillageState newState)
    {
        for (int i = 0; i < villages.Count; i++)
        {
            if (villages[i].id == villageId)
            {
                if (villages[i].state == newState)
                    return;

                villages[i].state = newState;
                OnVillageStateChanged?.Invoke(villageId, newState);
                return;
            }
        }
        Debug.LogError($"Village ID not found: {villageId}");
    }

    public VillageState GetVillageState(string villageId)
    {
        for (int i = 0; i < villages.Count; i++)
        {
            Debug.Log($"LSMANAGER ->villages[i].id = {villages[i].id} vs VillageID: {villageId}");
            if (villages[i].id == villageId)
                return villages[i].state;
        }
        Debug.LogError($"Village ID not found: {villageId}");
        return VillageState.PreInvasion;
    }

    public void TriggerGlobalInvasion()
    {
        if (invasionStarted) return;
        invasionStarted = true;
        Debug.Log("Global Invasion Starting");
        for (int i = 0; i < villages.Count; i++)
        {
            //if (villages[i].id == "Village7") continue;

            villages[i].state = VillageState.Invaded;
            OnVillageStateChanged?.Invoke(villages[i].id, VillageState.Invaded);
        }

        OnGlobalInvasionStarted?.Invoke();
    }

    public string DetermineNextScene(string villageId)
    {
        if (villageId == "EXIT")
        {
            // return LevelSelector scene (or however you name it)
            return "LevelSelector";
            // OR if you stored LS scene name in Location earlier then return that
        }

        for (int i = 0; i < villages.Count; i++)
        {
            if (villages[i].id == villageId)
            {
                var data = villages[i];

                if (data.state == VillageState.PreInvasion)
                    return data.preSceneName;

                return data.postSceneName;
            }

        }
        Debug.LogError($"Village not found: {villageId}");
        return null;
    }
 }
