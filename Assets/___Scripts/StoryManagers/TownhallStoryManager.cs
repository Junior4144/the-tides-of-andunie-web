using UnityEngine;
using UnityEngine.Playables;
using Unity.Cinemachine;
using UnityEngine.SceneManagement;
using System.Collections;

public class TownhallStoryManager : MonoBehaviour
{
    const int CUTSCENE_CAMERA_PRIORITY = 20;
    const int DEFAULT_CAMERA_PRIORITY = 0;

    [SerializeField] private PlayableDirector _cutscene;
    [SerializeField] private CinemachineCamera _cutsceneCamera;
    [SerializeField] private GameObject _playerSpawner;
    [SerializeField] private AudioSource _backgroundChatter;

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
            ActivateCutsceneLogic();
    }

    private void ActivateCutsceneLogic()
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
        if (GlobalStoryManager.Instance.playTownhallCutscene && _cutscene)
        {
            LSManager.Instance.TriggerGlobalInvasion();
            GlobalStoryManager.Instance.SetBool("playLSInvasionCutscene", true);
            GlobalStoryManager.Instance.SetBool("showWaypoints", false);
            StartCutscene();
        }
        else
            StartGameplay();
    }

    private void StartCutscene()
    {
        _cutscene.Play();
        SetGameplayElementsActive(false);
    }

    private void StartGameplay()
    {
        GameManager.Instance.SetState(GameState.PeacefulGameplay);
        SetGameplayElementsActive(true);
    }

    private void OnCutsceneStarted(PlayableDirector director)
    {
        GameManager.Instance.SetState(GameState.Cutscene);
        SetCutsceneCameraPriority(CUTSCENE_CAMERA_PRIORITY);
        SetBackgroundChatterActive(false);
        Debug.Log("[TownhallStoryManager] Cutscene started");
    }

    private void OnCutsceneStopped(PlayableDirector director)
    {
        GameManager.Instance.SetState(GameState.PeacefulGameplay);
        SetCutsceneCameraPriority(DEFAULT_CAMERA_PRIORITY);
        SetGameplayElementsActive(true);
        Debug.Log("[TownhallStoryManager] Cutscene stopped");
    }

    private void SetCutsceneCameraPriority(int priority)
    {
        if (_cutsceneCamera)
            _cutsceneCamera.Priority = priority;
    }

    private void SetGameplayElementsActive(bool active)
    {
        SetPlayerSpawnerActive(active);
        SetBackgroundChatterActive(active);
    }

    private void SetPlayerSpawnerActive(bool active)
    {
        if (!_playerSpawner) return;

        _playerSpawner.SetActive(active);
        Debug.Log($"[TownhallStoryManager] Player spawner {(active ? "enabled" : "disabled")}");
    }

    private void SetBackgroundChatterActive(bool active)
    {
        if (!_backgroundChatter) return;

        if (active)
            _backgroundChatter.Play();
        else
            _backgroundChatter.Stop();

        Debug.Log($"[TownhallStoryManager] Background chatter {(active ? "enabled" : "disabled")}");
    }
}
