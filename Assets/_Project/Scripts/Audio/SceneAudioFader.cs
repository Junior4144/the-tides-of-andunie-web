using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class SceneAudioFader : MonoBehaviour
{
    public AudioMixer mixer;
    
    public AudioMixerSnapshot normalSnapshot;
    public AudioMixerSnapshot transitionSnapshot;

    public float fadeToTransition = 0.8f;
    public float WaitForSceneToLoad = 1f;
    public float fadeToNormal = 1.5f;

    private void Awake()
    {
        StartCoroutine(HandleSetup());
    }

    private IEnumerator HandleSetup()
    {
        yield return null;

        normalSnapshot.TransitionTo(fadeToNormal);
    }

    public void FadeAndLoad() => FadeRoutine();

    private void FadeRoutine()
    {
        Debug.Log("Transition Audio to fade");
        transitionSnapshot.TransitionTo(fadeToTransition);

        //normalSnapshot.TransitionTo(fadeToNormal);
    }
}
