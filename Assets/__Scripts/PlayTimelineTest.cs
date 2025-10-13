using UnityEngine;
using UnityEngine.Playables;
using Unity.Cinemachine;

public class PlayTimelineTest : MonoBehaviour
{
    [SerializeField] private PlayableDirector _timeline;
    [SerializeField] private CinemachineCamera _cutsceneCamera;

    void Start()
    {
        _timeline.stopped += OnCutsceneFinished;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _cutsceneCamera.Priority = 20;
            _timeline.Play();
        }
    }

    private void OnCutsceneFinished(PlayableDirector director)
    {   
        _cutsceneCamera.Priority = 0;
    }
}