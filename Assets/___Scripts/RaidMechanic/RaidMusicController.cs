using UnityEngine;
using System.Collections;

public class RaidMusicController : MonoBehaviour
{
    [SerializeField] private RaidController raidController;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip preWaveClip;
    [SerializeField] private AudioClip inProgressClip;
    [SerializeField] private AudioClip postRaidClip;
    
    private void Awake()
    {
        if (preWaveClip != null) preWaveClip.LoadAudioData();
        if (inProgressClip != null) inProgressClip.LoadAudioData();
        if (postRaidClip != null) postRaidClip.LoadAudioData();
    }

    private void OnEnable()
    {
        if (raidController == null)
        {
            Debug.LogWarning("RaidMusicController is missing its RaidController!", this);
            return;
        }

        raidController.OnRaidReset += Stop;
        raidController.OnRaidTriggered += PlayPreWave;
        raidController.OnRaidStart += PlayInProgress;
        raidController.OnRaidComplete += PlayPostRaid;
        raidController.OnRaidFailed += PlayPostRaid;
    }


    private void OnDisable()
    {
        if (raidController == null) return;

        raidController.OnRaidReset -= Stop;
        raidController.OnRaidTriggered -= PlayPreWave;
        raidController.OnRaidStart -= PlayInProgress;
        raidController.OnRaidComplete -= PlayPostRaid;
        raidController.OnRaidFailed -= PlayPostRaid;
    }

    private void PlayPreWave()
    {
        Play(preWaveClip, loop: false);
    }

    private void PlayInProgress()
    {
        Play(inProgressClip, loop: true);
    }

    private void PlayPostRaid()
    {
        Play(postRaidClip, loop: false);
    }

    private void Stop() => audioSource.Stop();

    private void Play(AudioClip clip, bool loop)
    {
        if (clip == null || audioSource == null) return;
        audioSource.loop = loop;
        audioSource.clip = clip;
        audioSource.Play();
    }
}