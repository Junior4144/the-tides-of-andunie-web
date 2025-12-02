using UnityEngine;

public class TownHallStoryBoolSetter : MonoBehaviour
{
    private void Start()
    {
        GlobalStoryManager.Instance.SetBool("HasTalkedToChief", true);
        GlobalStoryManager.Instance.SetBool("enterLevelSelectorFirstTime", true);
    }
}
