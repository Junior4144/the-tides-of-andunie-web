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

    public void FadeAndLoad() =>
        StartCoroutine(FadeRoutine());

    private IEnumerator FadeRoutine()
    {
        Debug.Log("Transition Audio to fade");
        transitionSnapshot.TransitionTo(fadeToTransition);
        yield return new WaitForSecondsRealtime(WaitForSceneToLoad);

        normalSnapshot.TransitionTo(fadeToNormal);
    }
}
