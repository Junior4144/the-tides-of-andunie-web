using UnityEngine;

public class TownHallStoryBoolSetter : MonoBehaviour
{
    private void Start()
    {
        GlobalStoryManager.Instance.HasTalkedToChief = true;
        GlobalStoryManager.Instance.enterLevelSelectorFirstTime = true;
    }
}
