using System.Collections.Generic;
using UnityEngine;

public class RewardsUIDebugger : MonoBehaviour
{
    [SerializeField] private RewardUIController _rewardUIController;
    [SerializeField] private KeyCode _key;
    [SerializeField] private List<RewardListing> _rewards;

    private bool _isOpen = false;

    void Update()
    {
        if (Input.GetKeyDown(_key))
            if (!_isOpen)
                _rewardUIController.ShowRewards(_rewards);
            else
                _rewardUIController.HideRewards();

            _isOpen = !_isOpen;
    }           
}
