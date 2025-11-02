using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewRewardTable", menuName = "Raids/Rewards Config")]
public class RewardsConfig : ScriptableObject
{
    public List<RewardListing> RewardItems;
}