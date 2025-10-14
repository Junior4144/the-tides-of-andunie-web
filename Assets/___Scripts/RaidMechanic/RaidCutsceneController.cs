using UnityEngine;
using UnityEngine.Playables;
using System;
using Unity.Cinemachine;

public class RaidCutsceneController : MonoBehaviour
{
    [SerializeField] private PlayableDirector _introCutscene;
    [SerializeField] private PlayableDirector _outroCutscene;

    [SerializeField] private CinemachineCamera _introCutsceneCamera;
    [SerializeField] private CinemachineCamera _outroCutsceneCamera;
    
    public event Action OnIntroCutsceneFinished;
    public event Action OnOutroCutsceneFinished;
    
    void Start()
    {
        _introCutscene.stopped += OnIntroStopped;
        _outroCutscene.stopped += OnOutroStopped;
    }
    
    void OnDestroy()
    {
        _introCutscene.stopped -= OnIntroStopped;
        _outroCutscene.stopped -= OnOutroStopped;
    }
    
    public void PlayIntroCutscene()
    {
        GameManager.Instance.SetState(GameState.Cutscene);
        PrioritizeCamera(_introCutsceneCamera);
        _introCutscene.Play();
    }
    
    public void PlayOutroCutscene()
    {
        GameManager.Instance.SetState(GameState.Cutscene);
        PrioritizeCamera(_outroCutsceneCamera);
        _outroCutscene.Play();
    }
    
    private void OnIntroStopped(PlayableDirector director)
    {
        GameManager.Instance.SetState(GameState.Gameplay);
        DePrioritizeCamera(_introCutsceneCamera);
        OnIntroCutsceneFinished?.Invoke();  
    }

    private void OnOutroStopped(PlayableDirector director)
    {
        GameManager.Instance.SetState(GameState.Gameplay);
        DePrioritizeCamera(_outroCutsceneCamera);
        OnOutroCutsceneFinished?.Invoke();
    }

    private void PrioritizeCamera(CinemachineCamera camera) => camera.Priority = 20;

    private void DePrioritizeCamera(CinemachineCamera camera) => camera.Priority = 0;
}