using UnityEngine;
using UnityEngine.Playables;
using Unity.Cinemachine;

public class TownhallStoryManager : MonoBehaviour
{
    const int CUTSCENE_CAMERA_PRIORITY = 20;
    const int DEFAULT_CAMERA_PRIORITY = 0;

    [SerializeField] private PlayableDirector _cutscene;
    [SerializeField] private CinemachineCamera _cutsceneCamera;
    [SerializeField] private GameObject _playerSpawner;
    [SerializeField] private AudioSource _backgroundChatter;

    void Start()
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
            StartCutscene();
        }
        else
            StartGameplay();
    }

    private void StartCutscene()
    {
        _cutscene.Play();
        SetGameplayElementsActive(false);
        GlobalStoryManager.Instance.playTownhallCutscene = false;
    }

    private void StartGameplay()
    {
        GameManager.Instance.SetState(GameState.Gameplay);
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
        GameManager.Instance.SetState(GameState.Gameplay);
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
