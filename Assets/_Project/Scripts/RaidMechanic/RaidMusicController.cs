using UnityEngine;
using System.Collections;
using System;
using Random = UnityEngine.Random;
using System.Collections.Generic;
using System.Linq;

public class RaidMusicController : MonoBehaviour
{
    [SerializeField] private RaidController raidController;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] preRaidClips;
    [SerializeField] private AudioClip[] inProgressClips;
    [SerializeField] private AudioClip[] postRaidClips;
    
    private void Awake()
    {
        if (preRaidClips != null) foreach (var clip in preRaidClips) clip.LoadAudioData();
        if (inProgressClips != null) foreach (var clip in inProgressClips) clip.LoadAudioData();
        if (postRaidClips != null) foreach (var clip in postRaidClips) clip.LoadAudioData();
    }

    private void OnEnable()
    {
        if (raidController == null)
        {
            Debug.LogWarning("RaidMusicController is missing its RaidController!", this);
            return;
        }

        raidController.OnRaidReset += Stop;
        RaidController.OnRaidTriggered += PlayPreRaid;
        RaidController.OnRaidStart += PlayInProgress;
        raidController.OnRaidComplete += PlayPostRaid;
        raidController.OnRaidFailed += PlayPostRaid;
    }


    private void OnDisable()
    {
        if (raidController == null) return;

        raidController.OnRaidReset -= Stop;
        RaidController.OnRaidTriggered -= PlayPreRaid;
        RaidController.OnRaidStart -= PlayInProgress;
        raidController.OnRaidComplete -= PlayPostRaid;
        raidController.OnRaidFailed -= PlayPostRaid;
    }

    private void PlayPreRaid()
    {
        Play(RandomClip(preRaidClips), loop: false);
    }

    private void PlayInProgress()
    {
        Play(RandomClip(inProgressClips), loop: true);
    }

    private void PlayPostRaid()
    {
        Play(RandomClip(postRaidClips), loop: false);
    }

    private AudioClip RandomClip(AudioClip[] clips) => clips?.Length > 0 ? clips[Random.Range(0, clips.Length)] : null;

    private void Stop() => audioSource.Stop();

    private void Play(AudioClip clip, bool loop)
    {
        if (clip == null || audioSource == null) return;
        audioSource.loop = loop;
        audioSource.clip = clip;
        audioSource.Play();
    }
}