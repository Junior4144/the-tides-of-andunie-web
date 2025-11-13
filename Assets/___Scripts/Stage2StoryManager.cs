using UnityEngine;

public class Stage2StoryManager : MonoBehaviour
{
    private void OnEnable()
    {
        RaidRewardManager.OnRewardCollected += HandleRewardCollected;
    }
    private void OnDisable()
    {
        RaidRewardManager.OnRewardCollected -= HandleRewardCollected;
    }

    private void HandleRewardCollected()
    {
        PlayerManager.Instance.HandleDestroy();
    }
}
