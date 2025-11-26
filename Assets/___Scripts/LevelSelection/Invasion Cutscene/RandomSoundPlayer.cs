using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class RandomSoundPlayer : MonoBehaviour
{
    [SerializeField] private AudioClip[] audioClips;

    private AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        PlayRandomSound();
    }

    private void PlayRandomSound()
    {
        if (audioClips == null || audioClips.Length == 0)
        {
            Debug.LogWarning("[RandomSoundPlayer] No audio clips assigned");
            return;
        }

        var randomClip = audioClips[Random.Range(0, audioClips.Length)];
        audioSource.clip = randomClip;
        audioSource.Play();
    }
}
