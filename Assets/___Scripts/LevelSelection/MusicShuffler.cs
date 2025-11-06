using UnityEngine;



[RequireComponent (typeof(AudioSource))]
public class MusicShuffler : MonoBehaviour
{
    private AudioSource _audioSource;
    public AudioClip[] clips;
    int i;

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();

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
