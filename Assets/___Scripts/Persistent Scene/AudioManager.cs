using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    private SceneAudioFader _sceneAudioFader;
    public AudioMixer MasterAudioMixer;
    private AudioSource audioSource;

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;

        _sceneAudioFader = GetComponent<SceneAudioFader>();
        audioSource = GetComponent<AudioSource>();
    }

    public void FadeAudio() =>
        _sceneAudioFader.FadeAndLoad();

    public void PlayOneShot(AudioClip clip, float volumeScale = 1f)
    {
        if (clip != null)
            audioSource.PlayOneShot(clip, volumeScale);
        else
            Debug.LogWarning("[AudioPlayer] AudioClip is null. Skipping sound.");
    }
}