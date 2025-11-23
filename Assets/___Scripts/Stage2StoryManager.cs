using System.Collections.Generic;
using NavMeshPlus.Extensions;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Playables;

public class Stage2StoryManager : MonoBehaviour
{

    [SerializeField] private PlayableDirector _introCutscene;
    [SerializeField] private List<GameObject> _cutsceneEnemies;

    private void OnEnable()
    {
        RaidRewardManager.OnRewardCollected += HandleRewardCollected;
        _introCutscene.stopped += ActivateCutsceneEnemies; 
    }
    private void OnDisable()
    {
        RaidRewardManager.OnRewardCollected -= HandleRewardCollected;
        _introCutscene.stopped -= ActivateCutsceneEnemies; 
    }

    private void HandleRewardCollected()
    {
        PlayerManager.Instance.HandleDestroy();
    }

    private void ActivateCutsceneEnemies(PlayableDirector director)
    {
        foreach (var enemy in _cutsceneEnemies)
        {
            if (enemy.TryGetComponent<NavMeshAgent>(out var agent))
                agent.enabled = true;
                enemy.SetActive(true);
        }

    }
}
