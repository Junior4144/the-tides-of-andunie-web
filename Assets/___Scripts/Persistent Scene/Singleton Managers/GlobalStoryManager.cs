using UnityEngine;

public class GlobalStoryManager : MonoBehaviour
{

    public static GlobalStoryManager Instance;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    #region Level1Townhall
    public bool playTownhallCutscene = true;
    #endregion

    #region YourSceneBoolsHere
  
    #endregion
}

