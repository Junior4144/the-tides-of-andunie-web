using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    private SceneAudioFader _sceneAudioFader;
    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;

        _sceneAudioFader = GetComponent<SceneAudioFader>();
    }
    public void FadeAudio() =>
        _sceneAudioFader.FadeAndLoad();

}