using UnityEngine;

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
}

