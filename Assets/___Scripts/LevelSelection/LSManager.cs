using System;
using System.Collections.Generic;
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
    public VillageState state = VillageState.PreInvasion;
    public string SceneName;

}

public class LSManager : MonoBehaviour
{
    public static LSManager Instance;

    [SerializeField] private List<VillageData> villages = new List<VillageData>();

    private bool invasionStarted = false;
    public bool startGlobalInvasion = false;

    public event Action<string, VillageState> OnVillageStateChanged;
    public static event Action OnGlobalInvasionStarted;

    public bool HasInvasionStarted => invasionStarted;

    public bool globalInvasionEventSent = false;

    void Awake()
    {
        if (Instance != null && Instance != this) {Destroy(gameObject); return;}
        Instance = this;

    }

    private void OnDisable()
    {
        InSceneActivationManager.OnSceneActivated -= LevelSelectorActive;
    }


    private void Start()
    {
        if (startGlobalInvasion)
            TriggerGlobalInvasion();
            InSceneActivationManager.OnSceneActivated += LevelSelectorActive;

        if (SceneManager.GetActiveScene().name == "LevelSelector")
        {
            Debug.Log("LSManager started inside LevelSelector — manual invoke");
            LevelSelectorActive();
        }

    }
    private void LevelSelectorActive()
    {
        if (globalInvasionEventSent)
            return;

        globalInvasionEventSent = true;

        Debug.LogError("Handling Global Invoke");
        OnGlobalInvasionStarted?.Invoke();
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
            if (villages[i].id == "Village7") continue;

            villages[i].state = VillageState.Invaded;
            OnVillageStateChanged?.Invoke(villages[i].id, VillageState.Invaded);
        }

        if (!globalInvasionEventSent)
        {
            globalInvasionEventSent = true;
            OnGlobalInvasionStarted?.Invoke();
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
 }
