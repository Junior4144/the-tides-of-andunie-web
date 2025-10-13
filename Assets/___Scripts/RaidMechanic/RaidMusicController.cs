using UnityEngine;

public class RaidMusicController : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip preWaveClip;
    [SerializeField] private AudioClip inProgressClip;
    [SerializeField] private AudioClip postRaidClip;

    public void PlayPreWave()
    {
        Play(preWaveClip, loop: false);
    }

    public void PlayInProgress()
    {
        Play(inProgressClip, loop: true);
    }

    public void PlayPostRaid()
    {
        Play(postRaidClip, loop: false);
    }

    public void Stop() => audioSource.Stop();

    private void Play(AudioClip clip, bool loop)
    {
        if (clip == null || audioSource == null) return;
        audioSource.loop = loop;
        audioSource.clip = clip;
        audioSource.Play();
    }
}
