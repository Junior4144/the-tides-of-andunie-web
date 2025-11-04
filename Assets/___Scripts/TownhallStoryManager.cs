using UnityEngine;
using UnityEngine.Playables;
using Unity.Cinemachine;

public class TownhallStoryManager : MonoBehaviour
{
    [SerializeField] private bool _playCutscene = false;
    [SerializeField] private PlayableDirector _cutscene;
    [SerializeField] private CinemachineCamera _cutsceneCamera;
    [SerializeField] private GameObject _playerSpawner;
    [SerializeField] private AudioSource _backgroundChatter;

    void Start()
    {
        if (_cutscene)
        {
            _cutscene.played += OnCutsceneStarted;
            _cutscene.stopped += OnCutsceneStopped;
        }

        if (_playCutscene && _cutscene)
        {
            _cutscene.Play();
            SetPlayerSpawnerActive(false);
            SetBackgroundChatterActive(false);
        }
        else
        {
            SetPlayerSpawnerActive(true);
            SetBackgroundChatterActive(true);
        }
    }

    void OnDestroy()
    {
        if (_cutscene)
        {
            _cutscene.played -= OnCutsceneStarted;
            _cutscene.stopped -= OnCutsceneStopped;
        }
    }

    private void OnCutsceneStarted(PlayableDirector director)
    {
        GameManager.Instance.SetState(GameState.Cutscene);

        if (_cutsceneCamera)
            _cutsceneCamera.Priority = 20;

        SetBackgroundChatterActive(false);

        Debug.Log("[TownhallStoryManager] Cutscene started");
    }

    private void OnCutsceneStopped(PlayableDirector director)
    {
        GameManager.Instance.SetState(GameState.Gameplay);

        if (_cutsceneCamera)
            _cutsceneCamera.Priority = 0;

        SetPlayerSpawnerActive(true);
        SetBackgroundChatterActive(true);

        Debug.Log("[TownhallStoryManager] Cutscene stopped");
    }

    private void SetPlayerSpawnerActive(bool active)
    {
        if (_playerSpawner)
        {
            _playerSpawner.SetActive(active);
            Debug.Log($"[TownhallStoryManager] Player spawner {(active ? "enabled" : "disabled")}");
        }
    }

    private void SetBackgroundChatterActive(bool active)
    {
        if (_backgroundChatter)
        {
            if (active)
                _backgroundChatter.Play();
            else
                _backgroundChatter.Stop();

            Debug.Log($"[TownhallStoryManager] Background chatter {(active ? "enabled" : "disabled")}");
        }
    }
}
