using UnityEngine;
using UnityEngine.Playables;
using System;
using Unity.Cinemachine;

public class RaidCutsceneController : MonoBehaviour
{
    [Header("Raid Connection")]
    [SerializeField] private RaidController raidController;
    [SerializeField] private bool skipIntroCutscene = false;
    [SerializeField] private bool skipOutroCutscene = false;

    [Header("Cutscene Assets")]
    [SerializeField] private PlayableDirector _introCutscene;
    [SerializeField] private PlayableDirector _outroCutscene;
    [SerializeField] private CinemachineCamera _introCutsceneCamera;
    [SerializeField] private CinemachineCamera _outroCutsceneCamera;


    void Start()
    {
        if (raidController == null)
        {
            Debug.LogWarning("RaidCutsceneController has no RaidController assigned!", this);
            return;
        }
        raidController.OnRaidTriggered += HandleRaidTriggered;
        if (RewardsExist())
            RaidRewardManager.Instance.OnRewardCollected += PlayOutroCutscene;
        else
            raidController.OnRaidComplete += PlayOutroCutscene;
        
        raidController.OnRaidFailed += PlayOutroCutscene;
        _introCutscene.stopped += OnIntroStopped;
        _outroCutscene.stopped += OnOutroStopped;
    }

    void OnDestroy()
    {
        if (raidController != null)
        {
            raidController.OnRaidTriggered -= HandleRaidTriggered;
            if (RewardsExist())
                RaidRewardManager.Instance.OnRewardCollected -= PlayOutroCutscene;
            else
                raidController.OnRaidComplete -= PlayOutroCutscene;
            
            raidController.OnRaidFailed -= PlayOutroCutscene;
        }

        _introCutscene.stopped -= OnIntroStopped;
        _outroCutscene.stopped -= OnOutroStopped;
    }


    private bool RewardsExist() => raidController.RaidCompletionRewards.Count > 0;

    private void HandleRaidTriggered()
    {
        if (skipIntroCutscene)
        {
            raidController.BeginRaidSequence();
        }
        else
        {
            PlayIntroCutscene();
        }
    }

    private void PlayIntroCutscene()
    {
        GameManager.Instance.SetState(GameState.Cutscene);
        PrioritizeCamera(_introCutsceneCamera);
        _introCutscene.Play();
    }

    private void PlayOutroCutscene()
    {
        if (skipOutroCutscene) return;

        GameManager.Instance.SetState(GameState.Cutscene);
        PrioritizeCamera(_outroCutsceneCamera);
        _outroCutscene.Play();
    }

    private void OnIntroStopped(PlayableDirector director)
    {
        GameManager.Instance.SetState(GameState.Gameplay);
        DePrioritizeCamera(_introCutsceneCamera);
        raidController.BeginRaidSequence();
    }

    private void OnOutroStopped(PlayableDirector director)
    {
        GameManager.Instance.SetState(GameState.Gameplay);
        DePrioritizeCamera(_outroCutsceneCamera);
    }

    private void PrioritizeCamera(CinemachineCamera camera) => camera.Priority = 20;
    private void DePrioritizeCamera(CinemachineCamera camera) => camera.Priority = 0;
}