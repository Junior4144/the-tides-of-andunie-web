using System.Reflection;
using UnityEngine;
using static SaveGameManager;


public class GlobalStoryManager : MonoBehaviour
{

    public static GlobalStoryManager Instance;

    [HideInInspector] public string LastLiberatedVillageID = "";

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    #region Level1Townhall
    public bool playTownhallCutscene { get; private set; } = true;
    #endregion

    #region LevelSelector
    public bool enterLevelSelectorFirstTime { get; private set; } = false;
    public bool playLSInvasionCutscene { get; private set; } = false;
    public bool HasTalkedToChief { get; private set; } = false;
    public bool HasExitedLiberation { get; private set; } = false;
    #endregion

    #region WayPoint
    public bool showWaypoints { get; private set; } = false;
    #endregion

    #region GameLogic
    public bool comingFromPauseMenu { get; private set; } = false;
    #endregion

    public void SetBool(string booleanName, bool value)
    {
        FieldInfo field = typeof(GlobalStoryManager)
            .GetField(booleanName);

        if (field == null)
        {
            Debug.LogError($"[GlobalStoryManager] Bool '{booleanName}' not found!");
            return;
        }

        if (field.FieldType != typeof(bool))
        {
            Debug.LogError($"[GlobalStoryManager] Field '{booleanName}' exists but is NOT a bool!");
            return;
        }

        field.SetValue(this, value);
        SaveGameManager.Instance.SaveGame();
        Debug.Log($"[GlobalStoryManager] Set {booleanName} = {value}");
    }

    public StorySaveData GetSaveData()
    {
        return new StorySaveData
        {
            lastLiberatedVillageID = LastLiberatedVillageID,

            playTownhallCutscene = playTownhallCutscene,
            enterLevelSelectorFirstTime = enterLevelSelectorFirstTime,
            playLSInvasionCutscene = playLSInvasionCutscene,
            hasTalkedToChief = HasTalkedToChief,
            hasExitedLiberation = HasExitedLiberation,

            showWaypoints = showWaypoints,
            comingFromPauseMenu = comingFromPauseMenu
        };
    }

    public void ApplySaveData(StorySaveData data)
    {
        LastLiberatedVillageID = data.lastLiberatedVillageID;

        playTownhallCutscene = data.playTownhallCutscene;
        enterLevelSelectorFirstTime = data.enterLevelSelectorFirstTime;
        playLSInvasionCutscene = data.playLSInvasionCutscene;
        HasTalkedToChief = data.hasTalkedToChief;
        HasExitedLiberation = data.hasExitedLiberation;

        showWaypoints = data.showWaypoints;
        comingFromPauseMenu = data.comingFromPauseMenu;
    }
}

