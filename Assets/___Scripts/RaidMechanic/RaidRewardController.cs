using UnityEngine;

public class RaidRewardController : MonoBehaviour
{
    [SerializeField] public RaidController raidController;

    public void OnEnable()
    {
        raidController.OnRaidComplete += HandleRaidFinished;
    }

    public void OnDisable()
    {
        raidController.OnRaidComplete -= HandleRaidFinished;
    }

    private void HandleRaidFinished()
    {
        // this is when you should stick up the UI with the objects
        var rewards = raidController.RaidCompletionRewards;
    }
}