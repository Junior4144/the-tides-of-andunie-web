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
    public bool playTownhallCutscene = true;
    #endregion

    #region LevelSelector
    public bool enterLevelSelectorFirstTime = false;
    public bool playLSInvasionCutscene = false;
    public bool HasTalkedToChief = false;
    public bool HasExitedLiberation = false;
    #endregion

    #region WayPoint
    public bool showWaypoints = false;
    #endregion

    #region GameLogic
    public bool comingFromPauseMenu = false;
    #endregion

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

