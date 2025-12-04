using System;
using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class LSStoryManager : MonoBehaviour
{
    const int CUTSCENE_CAMERA_PRIORITY = 20;
    const int DEFAULT_CAMERA_PRIORITY = -1;

    [SerializeField] private PlayableDirector _cutscene;
    [SerializeField] private CinemachineCamera _cutsceneCamera;

    private void OnEnable() => SceneManager.activeSceneChanged += HandleCheck;

    private void OnDisable() => SceneManager.activeSceneChanged -= HandleCheck;

    private void HandleCheck(Scene oldScene, Scene newScene)
    {
        StartCoroutine(CheckAfterLoading(newScene));
    }

    private IEnumerator CheckAfterLoading(Scene newScene)
    {
        yield return null;

        if (newScene == gameObject.scene)
            StartStoryManager();
    }



    void StartStoryManager()
    {
        SubscribeToCutsceneEvents();
        InitializeScene();
    }

    void OnDestroy() => UnsubscribeFromCutsceneEvents();

    private void SubscribeToCutsceneEvents()
    {
        if (!_cutscene) return;

        _cutscene.played += OnCutsceneStarted;
        _cutscene.stopped += OnCutsceneStopped;
    }

    private void UnsubscribeFromCutsceneEvents()
    {
        if (!_cutscene) return;

        _cutscene.played -= OnCutsceneStarted;
        _cutscene.stopped -= OnCutsceneStopped;
    }

    private void InitializeScene()
    {
        if (GlobalStoryManager.Instance.playLSInvasionCutscene && _cutscene)
        {
            StartCutscene();
        }
        else
        {
            StartGameplay();
        }
            
    }

    private void StartCutscene()
    {
        _cutscene.Play();
        GlobalStoryManager.Instance.SetBool("playLSInvasionCutscene", false);
    }

    private void StartGameplay()
    {
        GameManager.Instance.SetState(GameState.LevelSelector);
    }

    private void OnCutsceneStarted(PlayableDirector director)
    {

        LSCameraManager.Instance.DisableCamera();

        GameManager.Instance.SetState(GameState.Cutscene);
        SetCutsceneCameraPriority(CUTSCENE_CAMERA_PRIORITY);
        Debug.Log("[LSStoryManager] Cutscene started");
    }

    private void OnCutsceneStopped(PlayableDirector director)
    {
        GameManager.Instance.SetState(GameState.LevelSelector);
        SetCutsceneCameraPriority(DEFAULT_CAMERA_PRIORITY);
        Debug.Log("[LSStoryManager] Cutscene stopped");

        LSCameraManager.Instance.EnableCamera();

    }

    private void SetCutsceneCameraPriority(int priority)
    {
        if (_cutsceneCamera)
            _cutsceneCamera.Priority = priority;
    }
}
