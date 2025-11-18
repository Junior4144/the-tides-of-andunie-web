using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    private SceneAudioFader _sceneAudioFader;

    public AudioMixer MasterAudioMixer;

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;

        _sceneAudioFader = GetComponent<SceneAudioFader>();
    }
    public void FadeAudio() =>
        _sceneAudioFader.FadeAndLoad();
}