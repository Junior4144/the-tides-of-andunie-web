using UnityEngine;
using UnityEngine.Playables;
using System;
using Unity.Cinemachine;

public class RaidCutsceneController : MonoBehaviour
{
    [SerializeField] private PlayableDirector _introCutscene;
    [SerializeField] private PlayableDirector _outroCutscene;

    [SerializeField] private CinemachineCamera _cutsceneCamera;
    
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
        PrioritizeCamera();
        _introCutscene.Play();
    }
    
    public void PlayOutroCutscene()
    {
        GameManager.Instance.SetState(GameState.Cutscene);
        PrioritizeCamera();
        _outroCutscene.Play();
    }
    
    private void OnIntroStopped(PlayableDirector director)
    {
        GameManager.Instance.SetState(GameState.Gameplay);
        DePrioritizeCamera();
        OnIntroCutsceneFinished?.Invoke();  
    }

    private void OnOutroStopped(PlayableDirector director)
    {
        GameManager.Instance.SetState(GameState.Gameplay);
        DePrioritizeCamera();
        OnOutroCutsceneFinished?.Invoke();
    }

    private void PrioritizeCamera() => _cutsceneCamera.Priority = 20;

    private void DePrioritizeCamera() => _cutsceneCamera.Priority = 0;
}