using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent (typeof(AudioSource))]
public class MusicShuffler : MonoBehaviour
{
    private AudioSource _audioSource;
    public AudioClip[] clips;
    int i;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

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
            HandlePlayingMusic();
    }

    private void HandlePlayingMusic()
    {
        _audioSource = GetComponent<AudioSource>();
        i = 0;
        PlayNext();
    }
    void Update()
    {
        if (!_audioSource.isPlaying) PlayNext();
    }

    void PlayNext()
    {
        _audioSource.clip = clips[i];
        _audioSource.Play();
        i = (i + 1) % clips.Length;
    }
}
